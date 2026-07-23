using UnityEngine;

public class RangedFireBehaviour : MonoBehaviour, IEnemyBehaviour
{
    [SerializeField] private float attackRange = 8f;
    [SerializeField] private float fireCooldown = 2f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private string projectilePoolTag = "Fireball";

    public float AttackRange => attackRange;

    private float cooldownTimer;

    public void Chase(Transform target)
    {
        // Guardian stays put — it doesn't chase, just faces the player
        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
    }

    public void Attack(Transform target)
    {
        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            FireProjectile();
            cooldownTimer = fireCooldown;
        }
    }

    private void FireProjectile()
    {
        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position + transform.forward;
        ObjectPool.Instance.SpawnFromPool(projectilePoolTag, spawnPos, transform.rotation);
    }
}
