using UnityEngine;
using System.Collections.Generic;

public class PlayerUpgrades : MonoBehaviour
{
    public static PlayerUpgrades Instance { get; private set; }

    public float FireRateMultiplier { get; private set; } = 1f;
    public float DamageReductionPercent { get; private set; } = 0f;
    public bool HasInfiniteAmmo { get; private set; } = false;

    public List<UpgradeData> ChosenUpgrades { get; private set; } = new List<UpgradeData>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ApplyUpgrade(UpgradeData upgrade)
    {
        ChosenUpgrades.Add(upgrade); // track for UI only

        ResetUpgradeValues(); // reset then reapply (no stacking)

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

        UpgradeTrackerUI.Instance?.Refresh();
    }

    private void ResetUpgradeValues()
    {
        FireRateMultiplier = 1f;
        DamageReductionPercent = 0f;
        HasInfiniteAmmo = false;
    }

    public void ResetAll()
    {
        ResetUpgradeValues();
        ChosenUpgrades.Clear();
        UpgradeTrackerUI.Instance?.Refresh();
    }
}