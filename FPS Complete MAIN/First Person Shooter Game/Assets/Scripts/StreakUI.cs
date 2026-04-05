using UnityEngine;
using TMPro;

public class StreakUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI streakText;
    [SerializeField] private GameObject streakPanel; // panel that shows/hides

    private void Start()
    {
        ScoreManager.Instance.OnStreakChanged += UpdateStreakUI;
        streakPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.OnStreakChanged -= UpdateStreakUI;
    }

    private void UpdateStreakUI(int streak)
    {
        if (streak <= 1)
        {
            streakPanel.SetActive(false);
            return;
        }

        streakPanel.SetActive(true);
        int multiplier = GetMultiplier(streak);

        streakText.text = streak >= 10 ? $"KILL STREAK x{streak}  x{multiplier} MULTIPLIER" :
                          streak >= 5  ? $"KILL STREAK x{streak}  x{multiplier} MULTIPLIER" :
                                         $"KILL STREAK x{streak}  x{multiplier} MULTIPLIER";
    }

    private int GetMultiplier(int streak)
    {
        if (streak >= 10) return 4;
        if (streak >= 5)  return 3;
        if (streak >= 3)  return 2;
        return 1;
    }
}