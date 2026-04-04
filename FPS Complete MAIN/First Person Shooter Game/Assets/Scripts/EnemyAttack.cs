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
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            playerHealth = playerObj.GetComponent<PlayerHealth>();
    }

    void Update()
    {
          if (playerHealth == null || agent == null) return;

    bool inRange = !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;

    if (inRange && Time.time >= lastAttackTime + attackCooldown)
        Attack();
    }

    private void Attack()
    {
        lastAttackTime = Time.time;
        playerHealth.TakeDamage(damage);
        animator.SetTrigger("Attack");
    }
}