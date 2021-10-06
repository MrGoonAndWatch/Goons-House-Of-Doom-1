using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public int Qty;
    public Item Item;
    public RectTransform RectTransform;

    void Start()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    public void DiscardItem()
    {
        Item = null;
    }

    public void Combine(ItemSlot itemB)
    {
        var comboResult = Item.Combine(itemB.Item);
        if (comboResult.ItemA == null)
        {
            DiscardItem();
        }
        else
        {
            Item = comboResult.ItemA;
        }

        if (comboResult.ItemB == null)
        {
            itemB.DiscardItem();
        }
        else
        {
            itemB.Item = comboResult.ItemB;
        }
    }
}
