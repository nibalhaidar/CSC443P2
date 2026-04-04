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
        switch (upgrade.type)
        {
            case UpgradeType.FireRate:
                FireRateMultiplier *= upgrade.fireRateMultiplier;
                Debug.Log($"Fire rate multiplier is now {FireRateMultiplier}");
                break;

            case UpgradeType.DamageReduction:
                DamageReductionPercent += upgrade.damageReductionPercent;
                DamageReductionPercent = Mathf.Clamp(DamageReductionPercent, 0f, 0.90f); // cap at 90%
                Debug.Log($"Damage reduction is now {DamageReductionPercent * 100}%");
                break;

            case UpgradeType.InfiniteAmmo:
                HasInfiniteAmmo = true;
                Debug.Log("Infinite ammo activated!");
                break;
        }
    }
}