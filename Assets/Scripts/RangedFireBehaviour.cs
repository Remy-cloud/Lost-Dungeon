using UnityEngine;

public class RangedFireBehaviour : MonoBehaviour, IEnemyBehaviour
{
    [SerializeField] private float attackRange = 8f;
    [SerializeField] private float fireCooldown = 2f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private string projectilePoolTag = "EnemyFireball";
    [SerializeField] private float modelForwardOffset = 0f;

    public float AttackRange => attackRange;

    private float cooldownTimer;
    private Transform currentTarget;

    public void Chase(Transform target)
    {
        currentTarget = target;
        FaceTarget(target);
    }

    public void Attack(Transform target)
    {
        currentTarget = target;
        FaceTarget(target);

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            FireProjectile(target);
            cooldownTimer = fireCooldown;
        }
    }

    private void FaceTarget(Transform target)
    {
        Vector3 lookPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(lookPos);
        transform.Rotate(0f, modelForwardOffset, 0f);
    }

    private void FireProjectile(Transform target)
    {
        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;

        // Aim precisely at the player's position, ignoring model rotation quirks
        Vector3 aimTarget = target.position + Vector3.up * 1f; // aim slightly up, toward player's body/chest
        Vector3 direction = (aimTarget - spawnPos).normalized;
        Quaternion aimRotation = Quaternion.LookRotation(direction);

        ObjectPool.Instance.SpawnFromPool(projectilePoolTag, spawnPos, aimRotation);
    }
}
