using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemies/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Stats")]
    public float health = 100f;
    public float speed = 3.5f;
    public float stoppingDistance = 1.5f;

    [Header("Animation")]
    public RuntimeAnimatorController animatorController;
}