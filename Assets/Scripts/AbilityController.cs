using UnityEngine;

public class AbilityController : MonoBehaviour
{
    [SerializeField] private MonoBehaviour fireballAbilityComponent;
    [SerializeField] private MonoBehaviour lightningAbilityComponent;

    private IAbility fireballAbility;
    private IAbility lightningAbility;

    private float fireballTimer;
    private float lightningTimer;

    void Awake()
    {
        fireballAbility = fireballAbilityComponent as IAbility;
        lightningAbility = lightningAbilityComponent as IAbility;
    }

    void Update()
    {
        fireballTimer -= Time.deltaTime;
        lightningTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.K) && fireballTimer <= 0f)
        {
            fireballAbility.Activate(transform);
            fireballTimer = fireballAbility.Cooldown;
        }

        if (Input.GetKeyDown(KeyCode.L) && lightningTimer <= 0f)
        {
            lightningAbility.Activate(transform);
            lightningTimer = lightningAbility.Cooldown;
        }
    }
}
