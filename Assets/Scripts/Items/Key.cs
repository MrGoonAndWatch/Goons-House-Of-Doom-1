public class Key : Item
{
    public KeyType KeyType;

    private UseKey _useKey;

    void Start()
    {
        _useKey = FindObjectOfType<UseKey>();
    }

    public override bool IsStackable()
    {
        return false;
    }

    public override int? GetMaxStackSize()
    {
        return null;
    }

    public override bool UseItem()
    {
        _useKey.Use(this);
        return false;
    }

    public override ComboResult Combine(Item otherItem)
    {
        return new ComboResult
        {
            ItemA = new Garbage(),
            ItemB = null,
        };
    }
}
