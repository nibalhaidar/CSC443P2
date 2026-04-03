using UnityEngine;
using StarterAssets;

public class Turret : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private TurretData data;

    [Header("References")]
    [SerializeField] private Transform head;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Projectile projectilePrefab;

    private ObjectPool<Projectile> projectilePool;
    private Transform player;
    private float nextFireTime;
    private float currentHealth;

    private void Start()
    {
        player = FindAnyObjectByType<FirstPersonController>().transform;
        projectilePool = new ObjectPool<Projectile>(projectilePrefab, transform, data.poolSize);
        currentHealth = data.health;
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > data.detectionRange) return;

        TrackPlayer();

        if (Time.time >= nextFireTime)
            Fire();
    }

    private void TrackPlayer()
    {
        Vector3 direction = (player.position - head.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        head.rotation = Quaternion.Slerp(head.rotation, targetRotation, data.rotationSpeed * Time.deltaTime);
    }

    private void Fire()
    {
        nextFireTime = Time.time + (1f / data.fireRate);
        Projectile projectile = projectilePool.Get(firePoint.position, firePoint.rotation);
        projectile.Fire(data.damage, ReturnProjectile);
    }

    // ✅ Takes damage just like Enemy
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Turret Health: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        gameObject.SetActive(false); // or play death VFX, then disable
    }

    private void ReturnProjectile(Projectile projectile)
    {
        projectilePool.Return(projectile);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.detectionRange);
    }
}