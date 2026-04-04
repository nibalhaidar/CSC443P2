using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float damage = 10f;
    public float attackCooldown = 1f;

    private float lastAttackTime = 0f;
    private PlayerHealth playerHealth;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            playerHealth = playerObj.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (playerHealth == null || agent == null) return;

        // Attack when NavMesh considers the enemy as having reached the player
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
                Attack();
        }
    }

    private void Attack()
    {
        lastAttackTime = Time.time;
        playerHealth.TakeDamage(damage);
    }
}