using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public PlayerInventory Inventory;

    private List<Item> _touchingItems;

    void Start()
    {
        if (Inventory == null)
        {
            Inventory = FindObjectOfType<PlayerInventory>();
        }

        _touchingItems = new List<Item>();
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
        if (_touchingItems.Any() &&
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
        Inventory.AddItem(item);
        item.gameObject.SetActive(false);

        _touchingItems.RemoveAll(i => i.GetInstanceID() == item.GetInstanceID());
    }
}
