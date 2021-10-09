using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public PlayerInventory Inventory;

    private List<Item> TouchingItems;

    void Start()
    {
        if (Inventory == null)
        {
            Inventory = FindObjectOfType<PlayerInventory>();
        }

        TouchingItems = new List<Item>();
    }

    void OnTriggerEnter(Collider c)
    {
        var item = c.GetComponent<Item>();
        if (item == null)
            return;

        TouchingItems.Add(item);
    }

    void OnTriggerExit(Collider c)
    {
        var item = c.GetComponent<Item>();
        if (item == null)
            return;

        TouchingItems.RemoveAll(i => i.GetInstanceID() == item.GetInstanceID());
    }
    
    void Update()
    {
        if (TouchingItems.Any() &&
            !Input.GetButton(GameConstants.Controls.Aim) &&
            Input.GetButtonDown(GameConstants.Controls.Action))
            PickupCurrentItem();
    }

    void PickupCurrentItem()
    {
        var validItems = TouchingItems.Where(i => i != null).ToArray();
        if (!validItems.Any())
            return;

        var item = validItems.Last();
        Inventory.AddItem(item);
        item.gameObject.SetActive(false);

        TouchingItems.RemoveAll(i => i.GetInstanceID() == item.GetInstanceID());
    }
}
