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

    [Header("Wave Settings")]
    [SerializeField] private float timeBetweenWaves = 3f;

    private int activeEnemies = 0;
    private int currentWaveIndex = 0;
    public System.Action OnWaveComplete;
    public System.Action OnAllWavesComplete;

    private void Start()
    {
        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
{
    while (currentWaveIndex < levels.Length)
    {
        Debug.Log($"Wave {currentWaveIndex + 1} starting...");
        yield return StartCoroutine(SpawnWave(levels[currentWaveIndex]));

        yield return new WaitUntil(() => activeEnemies <= 0);

        Debug.Log($"Wave {currentWaveIndex + 1} complete!");
        OnWaveComplete?.Invoke();

        currentWaveIndex++;
        

        if (currentWaveIndex < levels.Length)
{
    if (UpgradeUI.Instance == null)
    {
        Debug.LogError("UpgradeUI.Instance is null!");
        yield break;
    }

    bool upgradePicked = false;
    UpgradeUI.Instance.OnUpgradeChosen += () => upgradePicked = true;
    UpgradeUI.Instance.ShowUpgrades();

    while (!upgradePicked)
        yield return new WaitForSecondsRealtime(0.1f);

    UpgradeUI.Instance.OnUpgradeChosen = null;

    Debug.Log($"Next wave in {timeBetweenWaves} seconds...");
    yield return new WaitForSecondsRealtime(timeBetweenWaves);
}
    }

    Debug.Log("All waves complete!");
    OnAllWavesComplete?.Invoke();
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
        Enemy enemy = Instantiate(entry.enemyData.prefab, point.position, point.rotation);
        enemy.data = entry.enemyData;

        activeEnemies++;
        enemy.OnDied += HandleEnemyDied;
    }

    private void HandleEnemyDied(Enemy enemy)
    {
        enemy.OnDied -= HandleEnemyDied;
        Destroy(enemy.gameObject);

        activeEnemies--;
    }
}