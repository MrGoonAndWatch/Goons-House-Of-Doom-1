public class UnlimitedHandgun : Handgun
{
    protected override bool IsUnlimited()
    {
        return true;
    }

    public override string GetDescription()
    {
        return "It's a basic handgun... But unlimited.";
    }

    public override bool ShowQty()
    {
        return false;
    }

    public override float GetRateOfFire()
    {
        return 1.0f;
    }
}
