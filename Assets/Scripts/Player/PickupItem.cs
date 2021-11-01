using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public PlayerInventory Inventory;

    private PlayerStatus _playerStatus;

    private List<Item> _touchingItems;

    void Start()
    {
        if (Inventory == null)
        {
            Inventory = FindObjectOfType<PlayerInventory>();
        }

        _playerStatus = FindObjectOfType<PlayerStatus>();

        _touchingItems = new List<Item>();
    }

    public void ResetState()
    {
        _touchingItems.RemoveAll(i => true);
    }

    void OnTriggerEnter(Collider c)
    {
        var item = c.GetComponent<Item>();
        if (item == null)
            return;

        _touchingItems.Add(item);
    }

    void OnTriggerExit(Collider c)
    {
        var item = c.GetComponent<Item>();
        if (item == null)
            return;

        _touchingItems.RemoveAll(i => i.GetInstanceID() == item.GetInstanceID());
    }
    
    void Update()
    {
        if (_playerStatus.CanInteract() && _touchingItems.Any() &&
            !Input.GetButton(GameConstants.Controls.Aim) &&
            Input.GetButtonDown(GameConstants.Controls.Action))
            PickupCurrentItem();
    }

    void PickupCurrentItem()
    {
        var validItems = _touchingItems.Where(i => i != null).ToArray();
        if (!validItems.Any())
            return;

        var item = validItems.Last();
        var remainingQty = Inventory.AddItem(item);
        if (item.ItemId != 0 && remainingQty != item.QtyOnPickup)
        {
            var playerStatus = FindObjectOfType<PlayerStatus>();
            playerStatus.GrabItem(item.ItemId);
        }

        if (remainingQty == 0)
            item.gameObject.SetActive(false);
        else
            item.QtyOnPickup = remainingQty;

        _touchingItems.RemoveAll(i => i.GetInstanceID() == item.GetInstanceID());
    }
}
