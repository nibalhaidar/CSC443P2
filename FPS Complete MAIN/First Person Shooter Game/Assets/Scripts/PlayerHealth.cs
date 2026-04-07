using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI (Optional)")]
    public Slider healthBar;          // Drag a UI Slider here in Inspector
    public TextMeshProUGUI healthText;           // Or a UI Text element

    [Header("Death Settings")]
    public string deathSceneName = ""; // Scene to load on death (leave empty to just disable)
    public float deathDelay = 1.5f;

    [Header("Visual Feedback")]
    public GameObject damageFlashOverlay;  // Optional: a red UI Image that flashes on hit
    public float flashDuration = 0.1f;

    private bool isDead = false;
    

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    /// <summary>
    /// Call this from any enemy script to deal damage to the player.
    /// </summary>
 public void TakeDamage(float amount)
{
    if (isDead) return;

    if (PlayerUpgrades.Instance != null)
        amount *= (1f - PlayerUpgrades.Instance.DamageReductionPercent);

    currentHealth -= amount;
    currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

    UpdateHealthUI();

    CameraShake.Instance?.Shake(0.2f, 0.1f); // add this line

    if (damageFlashOverlay != null)
        StartCoroutine(FlashDamage());

    if (currentHealth <= 0)
        Die();
}

    /// <summary>
    /// Heal the player by a given amount.
    /// </summary>
    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        if (healthText != null)
            healthText.text = $"HP: {currentHealth}/{maxHealth}";
    }

private void Die()
{
    isDead = true;
    Debug.Log("Player has died.");

    var controller = GetComponent<CharacterController>();
    if (controller != null) controller.enabled = false;

    int score = ScoreManager.Instance != null ? ScoreManager.Instance.CurrentScore : 0;
    GameOverUI.Instance?.Show(score);

    // removed RestartAfterDelay — buttons handle it now
}

private System.Collections.IEnumerator RestartAfterDelay(float delay)
{
    yield return new WaitForSecondsRealtime(delay); // ← ignores timeScale
    Time.timeScale = 1f;
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}

private void HandleDeath()
{
    Time.timeScale = 1f; // unpause before reloading
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}

    private System.Collections.IEnumerator FlashDamage()
    {
        damageFlashOverlay.SetActive(true);
        yield return new WaitForSeconds(flashDuration);
        damageFlashOverlay.SetActive(false);
    }
}