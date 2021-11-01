public class Handgun : Weapon
{
    public override void PlaySfx()
    {
        SoundManager.PlayHandgunSfx();
    }

    public override bool IsHitscan()
    {
        return true;
    }

    public override float GetDamagePerHit()
    {
        return 6;
    }

    public override float GetRateOfFire()
    {
        return 2.0f;
    }

    public override string GetDescription()
    {
        return "It's a basic handgun.";
    }

    public override string GetPrefabPath()
    {
        return ItemPrefabFolderPath + "Handgun";
    }
}
