using UnityEngine;
using TMPro;

public class WaveCompleteUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private WaveSpawner waveSpawner;

    private void Start()
    {
        panel.SetActive(false);
        waveSpawner.OnWaveComplete += ShowWaveComplete;
    }

    private void ShowWaveComplete()
    {
        panel.SetActive(true);
        waveText.text = $"Wave {waveSpawner.CurrentWave - 1} Complete!";
        StartCoroutine(HideAfterDelay(2f));
    }

    private System.Collections.IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        panel.SetActive(false);
    }

    private void OnDestroy()
    {
        waveSpawner.OnWaveComplete -= ShowWaveComplete;
    }
}