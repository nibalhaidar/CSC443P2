using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int CurrentScore { get; private set; } = 0;
    public int KillStreak { get; private set; } = 0;

    private float streakResetTime = 3f; // seconds before streak resets
    private float lastKillTime;

    public System.Action<int> OnScoreChanged;
    public System.Action<int> OnStreakChanged; // UI subscribes to this

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // reset streak if player hasn't killed in time
        if (KillStreak > 0 && Time.time - lastKillTime >= streakResetTime)
        {
            KillStreak = 0;
            OnStreakChanged?.Invoke(KillStreak);
        }
    }

    public void AddScore(int amount)
    {
        KillStreak++;
        lastKillTime = Time.time;
        OnStreakChanged?.Invoke(KillStreak);

        // multiply score based on streak
        int multiplier = GetMultiplier();
        int finalScore = amount * multiplier;

        CurrentScore += finalScore;
        OnScoreChanged?.Invoke(CurrentScore);

        Debug.Log($"Kill streak: {KillStreak} | Multiplier: x{multiplier} | Score: {CurrentScore}");
    }

    private int GetMultiplier()
    {
        if (KillStreak >= 10) return 4;
        if (KillStreak >= 5)  return 3;
        if (KillStreak >= 3)  return 2;
        return 1;
    }
}