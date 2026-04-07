using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance;

    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

   public void Show(int score)
{
    scoreText.text = $"Score: {score}";
    panel.SetActive(true);
    Time.timeScale = 0f;
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
}
   public void Restart()
{
    Time.timeScale = 1f;
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}

  public void Quit()
{
    Time.timeScale = 1f;
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
    SceneManager.LoadScene(0);
}
}