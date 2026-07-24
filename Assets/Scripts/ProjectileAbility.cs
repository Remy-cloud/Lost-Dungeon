using UnityEngine;

public class ProjectileAbility : MonoBehaviour, IAbility
{
    [SerializeField] private float cooldown = 2f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private string projectilePoolTag = "Fireball";

    public float Cooldown => cooldown;

    public void Activate(Transform user)
    {
        Vector3 spawnPos = firePoint != null ? firePoint.position : user.position + user.forward;
        ObjectPool.Instance.SpawnFromPool(projectilePoolTag, spawnPos, user.rotation);

        Debug.Log($"{projectilePoolTag} ability fired!");
    }
}
