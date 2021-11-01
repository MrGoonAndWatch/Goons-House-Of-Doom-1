using UnityEngine;

public class DiningRoomKey : Key
{
    public override string GetDescription()
    {
        return "It's a key with a picture of some baked ziti on the handle.";
    }

    public override string GetPrefabPath()
    {
        return ItemPrefabFolderPath + "DiningRoomKey";
    }

    public override KeyType GetKeyType()
    {
        return KeyType.DiningRoom;
    }
}
