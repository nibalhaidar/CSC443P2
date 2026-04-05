using System;
using UnityEngine;
using UnityEngine.AI;
using StarterAssets;
using System.Collections;

public class Enemy : MonoBehaviour, IPoolable
{
    [Header("Data")]
    public EnemyData data;

    private NavMeshAgent agent;
    private FirstPersonController player;
    private Animator animator;

    private float currentHealth;
    private bool isDead = false; // add this

    public event Action<Enemy> OnDied;
    [Header("VFX")]
[SerializeField] private ParticleSystem deathVFX;

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

    public void ApplyData()
    {
        currentHealth = data.health;
        agent.speed = data.speed;
        agent.stoppingDistance = data.stoppingDistance;

        if (animator != null && data.animatorController != null)
            animator.runtimeAnimatorController = data.animatorController;
    }

    private void Update()
    {
        if (agent.isOnNavMesh && player != null)
        {
            agent.SetDestination(player.transform.position);

            if (animator != null)
                animator.SetFloat("Speed", agent.velocity.magnitude);
        }
    }
public void TakeDamage(float damage)
{
    if (isDead) return; // add this check

    currentHealth -= damage;
    Debug.Log(name + " Health: " + currentHealth);

    if (currentHealth <= 0)
        Die();
}

private void Die()
{
    if (isDead) return;
    isDead = true;

    if (data != null)
        ScoreManager.Instance?.AddScore(data.scoreValue);

    agent.isStopped = true;
    agent.enabled = false;

    if (animator != null)
        animator.SetTrigger("Death");

  if (deathVFX != null)
{
    deathVFX.transform.SetParent(null);
    deathVFX.gameObject.SetActive(true);
    deathVFX.Play();
    Destroy(deathVFX.gameObject, deathVFX.main.duration + 0.5f);

    // only hide mesh if this enemy has a death VFX
    foreach (Renderer r in GetComponentsInChildren<Renderer>())
        r.enabled = false;
}
    StartCoroutine(DeathDelay());
}
private IEnumerator DeathDelay()
{
    // wait for death animation to finish
    float deathAnimDuration = 2f; // match this to your animation length
    yield return new WaitForSeconds(deathAnimDuration);

    OnDied?.Invoke(this);
}

    // ✅ Empty — not using pool for enemies anymore
    public void OnGetFromPool() { }
    public void OnReturnFromPool() { }
}