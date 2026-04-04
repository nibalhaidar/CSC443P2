using UnityEngine;

public class AmmoPickup : Pickup
{
   protected override bool OnPickedUp(Collider player)
{
    // No need to pick up ammo if infinite ammo is active
    if (PlayerUpgrades.Instance != null && PlayerUpgrades.Instance.HasInfiniteAmmo)
        return false;

    ActiveWeapon activeWeapon = player.GetComponentInChildren<ActiveWeapon>();

    if (activeWeapon == null || activeWeapon.CurrentWeapon == null) return false;

    Weapon weapon = activeWeapon.CurrentWeapon;
    if (weapon.CurrentAmmo >= weapon.Data.maxAmmo) return false;

    weapon.RefillAmmo();
    return true;
}
}
