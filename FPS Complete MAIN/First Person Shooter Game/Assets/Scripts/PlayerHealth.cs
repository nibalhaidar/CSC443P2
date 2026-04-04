using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI (Optional)")]
    public Slider healthBar;          // Drag a UI Slider here in Inspector
    public Text healthText;           // Or a UI Text element

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

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();

        if (damageFlashOverlay != null)
            StartCoroutine(FlashDamage());

        Debug.Log($"Player took {amount} damage. Current HP: {currentHealth}");

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

        // Disable player controls
        var controller = GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;

        // Optionally disable scripts (e.g. FPS controller)
        // GetComponent<YourFPSController>()?.enabled = false;

        Invoke(nameof(HandleDeath), deathDelay);
    }

    private void HandleDeath()
{
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}

    private System.Collections.IEnumerator FlashDamage()
    {
        damageFlashOverlay.SetActive(true);
        yield return new WaitForSeconds(flashDuration);
        damageFlashOverlay.SetActive(false);
    }
}