using UnityEngine;

public abstract class Item: MonoBehaviour
{
    protected const string ItemPrefabFolderPath = "Items/";

    public abstract string GetPrefabPath();

    public abstract bool IsStackable();
    public abstract int? GetMaxStackSize();
    // TODO: Check if this is the right way to store this, can it be staticly defined by inherited classes?
    public Texture2D MenuIcon;

    public abstract bool UseItem();
    public abstract ComboResult Combine(Item otherItem);
}
