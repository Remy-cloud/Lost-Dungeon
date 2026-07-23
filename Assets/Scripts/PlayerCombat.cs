using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackDamage = 20f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private LayerMask enemyLayer;

    private PlayerAnimationJuice animJuice;

    void Awake()
    {
        animJuice = GetComponent<PlayerAnimationJuice>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
    }

    private void Attack()
    {
        animJuice?.PlayAttackJuice();

        Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward, attackRange, enemyLayer);

        Debug.Log($"Attack triggered — hit {hits.Length} target(s)");

        foreach (Collider hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null && !damageable.IsDead)
            {
                damageable.TakeDamage(attackDamage);
            }
        }
    }
}
