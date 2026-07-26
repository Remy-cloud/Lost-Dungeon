using UnityEngine;

public class IceGuardianBehaviour : MonoBehaviour, IEnemyBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 8f;
    [SerializeField] private float fireCooldown = 2f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private string projectilePoolTag = "IceShard";

    [Header("Defensive Cycle")]
    [SerializeField] private float guardDuration = 4f;
    [SerializeField] private float vulnerableDuration = 1.5f;
    [SerializeField] private float guardDamageMultiplier = 0.1f;

    public float AttackRange => attackRange;

    private float cooldownTimer;
    private float cycleTimer;
    private bool isGuarding = true;
    private Health health;

    void Awake()
    {
        health = GetComponent<Health>();
        cycleTimer = guardDuration;
        health.incomingDamageMultiplier = guardDamageMultiplier;
    }

    void Update()
    {
        cycleTimer -= Time.deltaTime;
        if (cycleTimer <= 0f)
        {
            isGuarding = !isGuarding;
            health.incomingDamageMultiplier = isGuarding ? guardDamageMultiplier : 1f;
            cycleTimer = isGuarding ? guardDuration : vulnerableDuration;

            Debug.Log($"[IceGuardian] Now {(isGuarding ? "GUARDING (reduced damage)" : "VULNERABLE (full damage)")}");
        }
    }

    public void Chase(Transform target)
    {
        Vector3 lookPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(lookPos);
    }

    public void Attack(Transform target)
    {
        Chase(target);

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            Debug.Log("[IceGuardian] FIRING NOW");
            FireIceShard(target);
            cooldownTimer = fireCooldown;
        }
    }

    private void FireIceShard(Transform target)
    {
        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        Vector3 aimTarget = target.position + Vector3.up * 1f;
        Vector3 direction = (aimTarget - spawnPos).normalized;
        Quaternion aimRotation = Quaternion.LookRotation(direction);

        GameObject spawned = ObjectPool.Instance.SpawnFromPool(projectilePoolTag, spawnPos, aimRotation);
        Debug.Log(spawned != null ? "[IceGuardian] SUCCESS - projectile spawned" : "[IceGuardian] FAILED - pool returned null");
    }
}
