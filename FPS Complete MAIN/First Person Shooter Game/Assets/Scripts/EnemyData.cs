using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemies/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Prefab")]
    public Enemy prefab;

    [Header("Stats")]
    public float health = 100f;
    public float speed = 3.5f;
    public float stoppingDistance = 1.5f;
      public int count;               // How many to spawn this wave
    public float spawnInterval = 1f; // Delay between spawns of this enemy type
     public int totalToSpawn;    

    [Header("Animation")]
    public RuntimeAnimatorController animatorController;
}