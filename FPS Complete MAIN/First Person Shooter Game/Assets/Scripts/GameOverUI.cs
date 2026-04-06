using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance;

    [SerializeField] private GameObject panel;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show()
    {
        panel.SetActive(true);
        Time.timeScale = 0f; // pause the game
    }
}