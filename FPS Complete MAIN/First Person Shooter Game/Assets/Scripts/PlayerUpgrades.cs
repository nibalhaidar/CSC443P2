using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    public static PlayerUpgrades Instance { get; private set; }

    // Active upgrade values
    public float FireRateMultiplier { get; private set; } = 1f;
    public float DamageReductionPercent { get; private set; } = 0f;
    public bool HasInfiniteAmmo { get; private set; } = false;

    private void Awake()
    {
        Instance = this;
    }

    public void ApplyUpgrade(UpgradeData upgrade)
{
    ResetUpgrades();
    switch (upgrade.type)
    {
        case UpgradeType.FireRate:
            FireRateMultiplier = upgrade.fireRateMultiplier;
            break;

        case UpgradeType.DamageReduction:
            DamageReductionPercent = Mathf.Clamp(upgrade.damageReductionPercent, 0f, 0.90f);
            break;

        case UpgradeType.InfiniteAmmo:
            HasInfiniteAmmo = true;
            break;

        case UpgradeType.HealthRestore:
            PlayerHealth playerHealth = FindAnyObjectByType<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.Heal(upgrade.healthRestoreAmount);
            break;
    }
}
    private void ResetUpgrades()
{
    FireRateMultiplier = 1f;
    DamageReductionPercent = 0f;
    HasInfiniteAmmo = false;
}
}