public class Garbage : Item
{
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
