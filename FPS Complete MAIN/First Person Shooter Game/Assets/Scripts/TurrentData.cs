using UnityEngine;

[CreateAssetMenu(fileName = "TurretData", menuName = "Enemies/Turret Data")]
public class TurretData : ScriptableObject
{
    [Header("Stats")]
    public float health = 50f;
    public float detectionRange = 20f;
    public float rotationSpeed = 5f;
    public float fireRate = 2f;
    public int damage = 1;
    public int poolSize = 10;
}