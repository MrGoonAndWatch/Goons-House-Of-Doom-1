public abstract class Weapon : Item
{
    public int Ammo;

    public abstract bool IsHitscan();
    public abstract float GetDamagePerHit();
    /// <summary>
    /// 
    /// </summary>
    /// <returns>The rate of fire in shots/second.</returns>
    public abstract float GetRateOfFire();

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
        if (playerStatus.EquipedWeapon == this)
            playerStatus.EquipedWeapon = null;
        else
            playerStatus.EquipedWeapon = this;
        var menu = FindObjectOfType<PlayerInventory>();
        menu.EquipDirty = true;
        return false;
    }

    public override ComboResult Combine(Item otherItem)
    {
        return new ComboResult
        {
            ItemA = this,
            ItemB = otherItem,
        };
    }

    public virtual bool ShowQty()
    {
        return true;
    }
}
