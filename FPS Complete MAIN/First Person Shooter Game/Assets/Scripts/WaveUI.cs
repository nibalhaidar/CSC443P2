using UnityEngine;
using TMPro;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private WaveSpawner waveSpawner;

    private void Start()
    {
          waveSpawner.OnWaveComplete += UpdateWaveText;
    waveText.text = $"Wave {waveSpawner.CurrentWave}"; 
    }

    private void OnDestroy()
    {
        waveSpawner.OnWaveComplete -= UpdateWaveText;
    }

    private void UpdateWaveText()
    {
        waveText.text = $"Wave {waveSpawner.CurrentWave}";
    }
}