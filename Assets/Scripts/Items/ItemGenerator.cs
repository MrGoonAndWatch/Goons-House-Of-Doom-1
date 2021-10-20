using System;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    public static ItemGenerator Instance;

    public GameObject ItemDumpster;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }

        Instance = this;
    }

    public static Item CreateItem(string itemType)
    {
        if (Instance == null)
        {
            Debug.LogError("Could not create item, no instance of ItemGenerator has been initialized!");
            throw new ApplicationException("Could not create item, no instance of ItemGenerator has been initialized!");
        }

        var resource = Resources.Load(itemType);
        var itemObj = Instantiate(resource, Instance.ItemDumpster.transform);
        var item = (itemObj as GameObject)?.GetComponent<Item>();
        if (item == null)
            throw new ApplicationException("Could not find item on resource for prefab " + itemType);
        return item;
    }

}
