public class WestHallKey : Key
{
    public override string GetDescription()
    {
        return "It's a small key. The tag reads 'West Hallway'.";
    }

    public override string GetPrefabPath()
    {
        return ItemPrefabFolderPath + "WestHallwayKey";
    }

    public override KeyType GetKeyType()
    {
        return KeyType.WestHallway;
    }
}
