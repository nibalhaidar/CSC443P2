using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private Enemy enemyPrefab;        // ✅ Was EnemyHealth — now Enemy
    [SerializeField] private int prewarmCount = 5;

    [Header("Spawn Settings")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int maxActiveEnemies = 10;

    private ObjectPool<Enemy> pool;                    // ✅ Was ObjectPool<EnemyHealth>

    private void Start()
    {
        pool = new ObjectPool<Enemy>(enemyPrefab, transform, prewarmCount);
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (pool.CountActive < maxActiveEnemies && spawnPoints.Length > 0)
                SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Enemy enemy = pool.Get(point.position, point.rotation);  // ✅ Was EnemyHealth
        enemy.OnDied += HandleEnemyDied;
    }

    private void HandleEnemyDied(Enemy enemy)                    // ✅ Was EnemyHealth
    {
        enemy.OnDied -= HandleEnemyDied;
        pool.Return(enemy);
    }
}