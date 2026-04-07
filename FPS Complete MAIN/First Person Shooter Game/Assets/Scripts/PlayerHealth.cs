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
    public Slider healthBar;
    public TextMeshProUGUI healthText;

    [Header("Death Settings")]
    public string deathSceneName = "";
    public float deathDelay = 1.5f;

    [Header("Visual Feedback")]
    public GameObject damageFlashOverlay;
    public float flashDuration = 0.1f;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;
        AudioManager.Instance.PlayHurt();

        if (PlayerUpgrades.Instance != null)
            amount *= (1f - PlayerUpgrades.Instance.DamageReductionPercent);

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();

        CameraShake.Instance?.Shake(0.2f, 0.1f);

        if (damageFlashOverlay != null)
            StartCoroutine(FlashDamage());

        if (currentHealth <= 0)
            Die();
    }

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

        Time.timeScale = 0f;

        var controller = GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;

        int score = ScoreManager.Instance != null ? ScoreManager.Instance.CurrentScore : 0;
        GameOverUI.Instance?.Show(score);
    }

    private System.Collections.IEnumerator FlashDamage()
    {
        damageFlashOverlay.SetActive(true);
        yield return new WaitForSeconds(flashDuration);
        damageFlashOverlay.SetActive(false);
    }
}