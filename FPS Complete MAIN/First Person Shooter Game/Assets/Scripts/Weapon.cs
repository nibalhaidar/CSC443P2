using UnityEngine;
using StarterAssets;
using Cinemachine;

public class Weapon : MonoBehaviour
{
    [SerializeField] WeaponData weaponData;
    [SerializeField] ParticleSystem muzzleFlash;
    private CinemachineImpulseSource impulseSource;

    int currentAmmo;
    public int CurrentAmmo => currentAmmo;
    public bool HasAmmo => currentAmmo > 0;
    public WeaponData Data => weaponData;

    private void Awake()
    {
        currentAmmo = weaponData.maxAmmo;
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

   public void Shoot()
{
    if (!HasAmmo) return;

    // Only consume ammo if infinite ammo upgrade is not active
    if (PlayerUpgrades.Instance == null || !PlayerUpgrades.Instance.HasInfiniteAmmo)
        currentAmmo--;

    muzzleFlash.Play();
    impulseSource.GenerateImpulse();
}

    public void RefillAmmo()
    {
        currentAmmo = weaponData.maxAmmo;
    }

}
