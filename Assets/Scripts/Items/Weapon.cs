﻿public abstract class Weapon : Item
{
    public int Ammo;

    protected virtual bool IsUnlimited()
    {
        return false;
    }

    public int GetAmmo()
    {
        return IsUnlimited() ? 1 : Ammo;
    }

    public void DecreaseAmmo(int amount = 1)
    {
        if (IsUnlimited()) return;
        Ammo -= amount;
    }

    public abstract void PlaySfx();
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
        playerStatus.EquipWeapon(this);
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
