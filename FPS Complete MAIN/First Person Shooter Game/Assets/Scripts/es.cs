using System.Collections;
using UnityEngine;

public class es : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Waves")]
    [SerializeField] private WaveData[] waves;

    private int currentWaveIndex = 0;

    [System.Obsolete]
    public void StartWave(int waveIndex)
    {
        if (waveIndex < 0 || waveIndex >= waves.Length) return;

        currentWaveIndex = waveIndex;
        StartCoroutine(RunWave(waves[waveIndex]));
    }

    [System.Obsolete]
    private IEnumerator RunWave(WaveData wave)
    {
        // Start spawning all enemy types concurrently
        foreach (EnemyData enemyData in wave.enemies)
        {
            StartCoroutine(SpawnEnemyType(enemyData));
        }

        // Wait until all enemies are destroyed before finishing the wave
        while (GameObject.FindObjectsOfType<Enemy>().Length > 0)
        {
            yield return null;
        }

        Debug.Log("Wave completed!");
        // You can automatically start next wave here if you want:
        // currentWaveIndex++;
        // if(currentWaveIndex < waves.Length) StartWave(currentWaveIndex);
    }

    private IEnumerator SpawnEnemyType(EnemyData data)
    {
        int spawned = 0;
        while (spawned < data.totalToSpawn)
        {
            SpawnEnemy(data.prefab);
            spawned++;
            yield return new WaitForSeconds(data.spawnInterval);
        }
    }

    private void SpawnEnemy(Enemy prefab)
    {
        if (spawnPoints.Length == 0) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Enemy enemy = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

        // Optional: handle death automatically
        enemy.OnDied += (e) => Destroy(e.gameObject);
    }
}