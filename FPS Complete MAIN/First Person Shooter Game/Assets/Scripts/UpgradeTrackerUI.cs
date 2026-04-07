using UnityEngine;
using TMPro;

public class UpgradeTrackerUI : MonoBehaviour
{
    public static UpgradeTrackerUI Instance;

    [SerializeField] private TextMeshProUGUI upgradeText;
    [SerializeField] private GameObject panel;
    [SerializeField] private float displayDuration = 2f;

    private Coroutine hideCoroutine;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Refresh()
    {
        if (PlayerUpgrades.Instance == null) return;

        var upgrades = PlayerUpgrades.Instance.ChosenUpgrades;

        if (upgrades.Count == 0)
        {
            panel.SetActive(false);
            return;
        }

        UpgradeData latest = upgrades[upgrades.Count - 1];
        upgradeText.text = $"{latest.upgradeName}";
        panel.SetActive(true);

        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    private System.Collections.IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        panel.SetActive(false);
    }
}