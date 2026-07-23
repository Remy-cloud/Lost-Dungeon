using UnityEngine;

public interface IEnemyBehaviour
{
    void Chase(Transform target);
    void Attack(Transform target);
    float AttackRange { get; }
}
