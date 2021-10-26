public class Garbage : Item
{
    public override string GetDesription()
    {
        return "It's a useless pile of junk.";
    }

    public override string GetPrefabPath()
    {
        return ItemPrefabFolderPath + "Garbage";
    }

    public override bool IsStackable()
    {
        return true;
    }

    public override int? GetMaxStackSize()
    {
        return 99;
    }

    public override bool UseItem()
    {
        var playerStatus = FindObjectOfType<PlayerStatus>();
        playerStatus.SetHealth(1);
        return true;
    }

    public override ComboResult Combine(Item otherItem)
    {
        return new ComboResult
        {
            ItemA = this,
            ItemB = otherItem,
        };
    }
}
