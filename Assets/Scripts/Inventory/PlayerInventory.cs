using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Item[] Items;
    
    void Start()
    {
        Items = new Item[8];
    }
    
    void Update()
    {
        
    }
}
