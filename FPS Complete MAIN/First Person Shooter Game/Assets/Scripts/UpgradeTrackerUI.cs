using UnityEngine;
using TMPro;

public class UpgradeTrackerUI : MonoBehaviour
{
    public static UpgradeTrackerUI Instance;

    [SerializeField] private TextMeshProUGUI upgradeText;

    private void Awake()
    {
        Instance = this;
    }

    public void Refresh()
    {
        if (PlayerUpgrades.Instance == null) return;

        var upgrades = PlayerUpgrades.Instance.ChosenUpgrades;

        if (upgrades.Count == 0)
        {
            upgradeText.text = "";
            return;
        }

        // Only show the most recently picked upgrade
        UpgradeData latest = upgrades[upgrades.Count - 1];
        upgradeText.text = $"- {latest.upgradeName}";
    }
}