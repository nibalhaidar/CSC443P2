using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnEntry
    {
        public EnemyData enemyData;
        public int count;
        public float spawnInterval = 1f;
        public Transform[] spawnPoints;
    }

    [System.Serializable]
    public class LevelWave
    {
        public int levelIndex;
        public EnemySpawnEntry[] enemies;
    }

    [Header("Setup")]
    [SerializeField] private LevelWave[] levels;

    private int activeEnemies = 0;
    public System.Action OnWaveComplete;

    private void Start()
    {
        int levelIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        LevelWave wave = System.Array.Find(levels, l => l.levelIndex == levelIndex);

        if (wave == null)
        {
            Debug.LogWarning("No wave data for level " + levelIndex);
            return;
        }

        StartCoroutine(SpawnWave(wave));
    }

    private IEnumerator SpawnWave(LevelWave wave)
    {
        foreach (var entry in wave.enemies)
        {
            for (int i = 0; i < entry.count; i++)
            {
                SpawnEnemy(entry);
                yield return new WaitForSeconds(entry.spawnInterval);
            }
        }
    }

    private void SpawnEnemy(EnemySpawnEntry entry)
    {
        if (entry.spawnPoints == null || entry.spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points for " + entry.enemyData.name);
            return;
        }

        if (entry.enemyData == null || entry.enemyData.prefab == null)
        {
            Debug.LogWarning("EnemyData or prefab is null!");
            return;
        }

        Transform point = entry.spawnPoints[Random.Range(0, entry.spawnPoints.Length)];

        // ✅ Same logic as EnemySpawner — prefab already has data assigned
        Enemy enemy = Instantiate(entry.enemyData.prefab, point.position, point.rotation);

        // ✅ Override data from wave entry in case prefab has different data
        enemy.data = entry.enemyData;

        activeEnemies++;
        enemy.OnDied += HandleEnemyDied;
    }

    private void HandleEnemyDied(Enemy enemy)
    {
        enemy.OnDied -= HandleEnemyDied;
        Destroy(enemy.gameObject);

        activeEnemies--;
        if (activeEnemies <= 0)
        {
            Debug.Log("Wave Complete!");
            OnWaveComplete?.Invoke();
        }
    }
}