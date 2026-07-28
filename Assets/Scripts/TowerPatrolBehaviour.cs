using UnityEngine;

public class TowerPatrolBehaviour : MonoBehaviour, IEnemyBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private float patrolDistance = 4f;
    [SerializeField] private float patrolSpeed = 2f;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float fireCooldown = 1.5f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private string projectilePoolTag = "Spike";

    public float AttackRange => attackRange;

    private Vector3 startPos;
    private float patrolTimer;
    private float cooldownTimer;
    private int direction = 1;

    void Awake()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // left-right movement, independent of Chase/Attack state
        transform.position += Vector3.right * direction * patrolSpeed * Time.deltaTime;

        patrolTimer += Time.deltaTime;
        float offset = Mathf.Abs((transform.position - startPos).x);
        if (offset >= patrolDistance)
        {
            direction *= -1;
        }
    }

    public void Chase(Transform target)
    {
        FaceTarget(target);
    }

    public void Attack(Transform target)
    {
        FaceTarget(target);

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            FireSpike(target);
            cooldownTimer = fireCooldown;
        }
    }

    private void FaceTarget(Transform target)
    {
        Vector3 lookPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(lookPos);
    }

    private void FireSpike(Transform target)
    {
        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        Vector3 aimTarget = target.position + Vector3.up * 1f;
        Vector3 direction = (aimTarget - spawnPos).normalized;
        Quaternion aimRotation = Quaternion.LookRotation(direction);

        ObjectPool.Instance.SpawnFromPool(projectilePoolTag, spawnPos, aimRotation);
    }
}
