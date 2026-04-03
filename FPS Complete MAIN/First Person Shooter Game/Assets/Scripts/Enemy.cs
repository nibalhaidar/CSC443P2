using System;
using UnityEngine;
using UnityEngine.AI;
using StarterAssets;

public class Enemy : MonoBehaviour, IPoolable
{
    [Header("Data")]
    public EnemyData data;

    private NavMeshAgent agent;
    private FirstPersonController player;
    private Animator animator;

    private float currentHealth;

    // 🔥 THIS replaces EnemyHealth event
    public event Action<Enemy> OnDied;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        player = FindAnyObjectByType<FirstPersonController>();

        if (data == null)
        {
            Debug.LogError("EnemyData NOT assigned!", gameObject);
            return;
        }

        ApplyData();
    }

    void ApplyData()
    {
        currentHealth = data.health;

        agent.speed = data.speed;
        agent.stoppingDistance = data.stoppingDistance;

        if (animator != null && data.animatorController != null)
        {
            animator.runtimeAnimatorController = data.animatorController;
        }
    }

    private void Update()
    {
        if (agent.isOnNavMesh && player != null)
        {
            agent.SetDestination(player.transform.position);

            if (animator != null)
            {
                animator.SetFloat("Speed", agent.velocity.magnitude);
            }
        }
    }

    // 🔥 DAMAGE
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(name + " Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        OnDied?.Invoke(this); // 🔥 IMPORTANT (pool uses this)
    }

    // 🔁 POOLING SYSTEM
    public void OnGetFromPool()

    {
         ApplyData(); // ✅ This resets currentHealth = data.health, speed, etc.
        currentHealth = data.health;

        if (agent != null)
        {
            agent.enabled = false;
            agent.enabled = true;
            agent.Warp(transform.position);
        }
    }

    public void OnReturnFromPool()
    {
        OnDied = null;
    }
}