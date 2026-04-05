using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrades/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    [TextArea] public string description;
    public Sprite icon;
    public UpgradeType type;

    [Header("Values")]
    public float fireRateMultiplier = 1f;    // e.g. 1.5 = 50% faster
    public float damageReductionPercent = 0f; // e.g. 0.25 = take 25% less damage
    public bool infiniteAmmo = false;
    public float healthRestoreAmount = 0f;
}

public enum UpgradeType
{
    FireRate,
    DamageReduction,
    InfiniteAmmo,
    HealthRestore
}