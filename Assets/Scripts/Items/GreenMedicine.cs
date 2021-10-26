public class GreenMedicine : Item
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override string GetDesription()
    {
        return "A special mixture of chemicals that treat injuries almost instantly.";
    }

    public override string GetPrefabPath()
    {
        return ItemPrefabFolderPath + "GreenMedicine";
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
        var playerStatus = FindObjectOfType<PlayerStatus>();
        playerStatus.AddHealth(GameConstants.GreenMedicineHp);
        return true;
    }

    public override ComboResult Combine(Item otherItem)
    {
        // TODO: Setup Combos w/ other items.
        return new ComboResult
        {
            ItemA = ItemGenerator.CreateItem(new Garbage().GetPrefabPath()),
            ItemB = null,
        };
    }
}
