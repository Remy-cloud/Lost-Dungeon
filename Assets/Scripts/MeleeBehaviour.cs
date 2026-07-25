using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MeleeBehaviour : MonoBehaviour, IEnemyBehaviour
{
    [Header("Melee Settings")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float attackDamage = 15f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private LayerMask targetLayer;

    public float AttackRange => attackRange;

    private CharacterController controller;
    private float cooldownTimer;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void Chase(Transform target)
    {
        Vector3 direction = (target.position - transform.position);
        direction.y = 0f;

        if (direction.magnitude > 0.1f)
        {
            direction.Normalize();
            controller.SimpleMove(direction * moveSpeed);
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    public void Attack(Transform target)
    {
        Vector3 lookDir = target.position - transform.position;
        lookDir.y = 0f;
        if (lookDir.magnitude > 0.1f)
            transform.rotation = Quaternion.LookRotation(lookDir.normalized);

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            DealDamage();
            cooldownTimer = attackCooldown;
        }
    }

    private void DealDamage()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward, attackRange, targetLayer);

        Debug.Log($"[MeleeBehaviour] Attack triggered — hit {hits.Length} target(s)");

        foreach (Collider hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null && !damageable.IsDead)
            {
                Debug.Log($"[MeleeBehaviour] Dealing {attackDamage} damage to {hit.gameObject.name}");
                damageable.TakeDamage(attackDamage);
            }
        }
    }
}
