public abstract class Key : Item
{
    private UseKey _useKey;

    void Start()
    {
        _useKey = FindObjectOfType<UseKey>();
    }

    public abstract KeyType GetKeyType();
    
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
        // HACK: when key is created by DataSaver Start() is not called and this doesn't get properly initialized.
        if (_useKey == null)
            _useKey = FindObjectOfType<UseKey>();
        _useKey.Use(this);
        return false;
    }

    public override ComboResult Combine(Item otherItem)
    {
        return new ComboResult
        {
            ItemA = ItemGenerator.CreateItem(new Garbage().GetPrefabPath()),
            ItemB = null,
        };
    }
}
