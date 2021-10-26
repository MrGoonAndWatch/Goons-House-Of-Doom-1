public class Typewriter : Item
{
    public override string GetDesription()
    {
        return "A typewriter. If I can find a place to put this down I could record my progress.";
    }

    public override string GetPrefabPath()
    {
        return ItemPrefabFolderPath + "Typewriter";
    }

    public override bool IsStackable()
    {
        return true;
    }

    public override int? GetMaxStackSize()
    {
        return 12;
    }

    public override bool UseItem()
    {
        // TODO: Make sure we're colliding with a save surface.
        var gameSaver = FindObjectOfType<SaveGame>();
        gameSaver.Open();
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
