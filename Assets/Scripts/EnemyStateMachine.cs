using UnityEngine;

public enum EnemyState { Idle, Chase, Attack, Dead }

[RequireComponent(typeof(Health))]
public class EnemyStateMachine : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float detectionRange = 12f;
    [SerializeField] private Transform player; // assign in Inspector, or auto-find by tag

    private EnemyState currentState = EnemyState.Idle;
    private IEnemyBehaviour behaviour;
    private Health health;

    void Awake()
    {
        behaviour = GetComponent<IEnemyBehaviour>(); // works with ANY Strategy implementation
        health = GetComponent<Health>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }
    }

    void OnEnable()
    {
        Health.OnDeath += HandleDeath;
    }

    void OnDisable()
    {
        Health.OnDeath -= HandleDeath;
    }

    void Update()
    {
        if (currentState == EnemyState.Dead || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Idle:
                if (distance <= detectionRange) currentState = EnemyState.Chase;
                break;

            case EnemyState.Chase:
                behaviour.Chase(player);
                if (distance <= behaviour.AttackRange) currentState = EnemyState.Attack;
                break;

            case EnemyState.Attack:
                if (distance > behaviour.AttackRange) { currentState = EnemyState.Chase; break; }
                behaviour.Attack(player);
                break;
        }
    }

    private void HandleDeath(Health deadHealth)
    {
        if (deadHealth == health)
        {
            currentState = EnemyState.Dead;

            GameManager.Instance?.NotifyEnemyDefeated();

            // TODO: disable visuals/collider, maybe trigger level-unlock reward here
            gameObject.SetActive(false);
        }
    }
}
