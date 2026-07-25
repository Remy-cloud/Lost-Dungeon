using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifetime = 4f;
    [SerializeField] private LayerMask targetLayer;

    private float timer;

    void OnEnable()
    {
        timer = 0f;
    }

    void Update()
    {
        //Debug.DrawRay(transform.position, transform.forward * 2f, Color.red);
        transform.position += transform.forward * speed * Time.deltaTime;

        timer += Time.deltaTime;
        if (timer >= lifetime)
            gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[Fireball] Trigger entered with: {other.gameObject.name}, layer: {LayerMask.LayerToName(other.gameObject.layer)}");

        if (((1 << other.gameObject.layer) & targetLayer) == 0)
        {
            Debug.Log("[Fireball] Layer did NOT match targetLayer — ignoring");
            return;
        }

        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable == null)
        {
            Debug.Log("[Fireball] No IDamageable found on this object");
            return;
        }

        if (damageable.IsDead)
        {
            Debug.Log("[Fireball] Target is already dead");
            return;
        }

        Debug.Log($"[Fireball] Dealing {damage} damage to {other.gameObject.name}");
        damageable.TakeDamage(damage);
        gameObject.SetActive(false);
    }
}
