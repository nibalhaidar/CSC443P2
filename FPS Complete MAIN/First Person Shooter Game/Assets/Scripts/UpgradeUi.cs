using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    public static UpgradeUI Instance { get; private set; }

    [Header("UI References")]
    public GameObject upgradePanel;
    public Button[] upgradeButtons;          // 3 buttons in the panel
   public TMP_Text[] upgradeNameTexts;
public TMP_Text[] upgradeDescTexts;    public Image[] upgradeIcons;             // Icon image on each button (optional)

    [Header("All Available Upgrades")]
    public UpgradeData[] allUpgrades;        // Drag your 3 UpgradeData assets here

    public System.Action OnUpgradeChosen;

    private void Awake()
    {
        Instance = this;
        upgradePanel.SetActive(false);
    }

    public void ShowUpgrades()
    {
        upgradePanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
          // Unlock and show cursor
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;


        // Pick 3 random upgrades (or just use all 3 if you only have 3)
        List<UpgradeData> offered = PickRandomUpgrades(3);

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            if (i >= offered.Count) { upgradeButtons[i].gameObject.SetActive(false); continue; }

            UpgradeData upgrade = offered[i];
            int index = i; // capture for lambda

            upgradeNameTexts[i].text = upgrade.upgradeName;
            upgradeDescTexts[i].text = upgrade.description;

            if (upgradeIcons != null && upgradeIcons.Length > i && upgrade.icon != null)
                upgradeIcons[i].sprite = upgrade.icon;

            upgradeButtons[i].onClick.RemoveAllListeners();
            upgradeButtons[i].onClick.AddListener(() => SelectUpgrade(upgrade));
        }
    }

    private void SelectUpgrade(UpgradeData upgrade)
    {
        AudioManager.Instance.PlayUpgradeSelect();
        PlayerUpgrades.Instance.ApplyUpgrade(upgrade);
        upgradePanel.SetActive(false);
        Time.timeScale = 1f; // Resume the game

           // Lock and hide cursor again
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
        OnUpgradeChosen?.Invoke();
    }

    private List<UpgradeData> PickRandomUpgrades(int count)
    {
        List<UpgradeData> pool = new List<UpgradeData>(allUpgrades);
        List<UpgradeData> result = new List<UpgradeData>();

        while (result.Count < count && pool.Count > 0)
        {
            int rand = Random.Range(0, pool.Count);
            result.Add(pool[rand]);
            pool.RemoveAt(rand);
        }

        return result;
    }
}