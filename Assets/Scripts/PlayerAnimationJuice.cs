using UnityEngine;

public class PlayerAnimationJuice : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform modelTransform; // the visual mesh child (not the root with CharacterController)

    [Header("Walk Bob Settings")]
    [SerializeField] private float bobFrequency = 8f;
    [SerializeField] private float bobHeight = 0.08f;

    [Header("Attack Settings")]
    [SerializeField] private float attackScalePunch = 1.2f;
    [SerializeField] private float attackDuration = 0.15f;

    [Header("Death Settings")]
    [SerializeField] private float deathTipDuration = 0.5f;

    private CharacterController controller;
    private Vector3 modelStartLocalPos;
    private Vector3 modelBaseScale;
    private bool isDead = false;
    private bool isAttacking = false;

    void Awake()
    {
        controller = GetComponentInParent<CharacterController>();

        if (modelTransform == null)
            modelTransform = transform; // fallback: use this object itself

        modelStartLocalPos = modelTransform.localPosition;
        modelBaseScale = modelTransform.localScale;
    }

    void OnEnable()
    {
        Health.OnDeath += HandleDeath;
    }

    void OnDisable()
    {
        Health.OnDeath -= HandleDeath;
    }

    private void HandleDeath(Health health)
    {
        if (health.gameObject == this.gameObject)
        {
            PlayDeathJuice();
        }
    }

    void Update()
    {
        if (isDead) return;

        HandleWalkBob();
    }

    private void HandleWalkBob()
    {
        if (isAttacking) return;

        // Detect movement by checking horizontal velocity via CharacterController
        Vector3 horizontalVelocity = new Vector3(controller.velocity.x, 0f, controller.velocity.z);
        bool isMoving = horizontalVelocity.magnitude > 0.1f;

        if (isMoving)
        {
            float bobOffset = Mathf.Sin(Time.time * bobFrequency) * bobHeight;
            modelTransform.localPosition = modelStartLocalPos + new Vector3(0f, bobOffset, 0f);
        }
        else
        {
            // Smoothly return to rest position when idle
            modelTransform.localPosition = Vector3.Lerp(modelTransform.localPosition, modelStartLocalPos, Time.deltaTime * 8f);
        }
    }

    public void PlayAttackJuice()
    {
        if (isDead || isAttacking) return;
        StartCoroutine(AttackPunchRoutine());
    }

    private System.Collections.IEnumerator AttackPunchRoutine()
    {
        isAttacking = true;
        float elapsed = 0f;
        Vector3 punchScale = modelBaseScale * attackScalePunch;

        // Punch out
        while (elapsed < attackDuration / 2f)
        {
            elapsed += Time.deltaTime;
            modelTransform.localScale = Vector3.Lerp(modelBaseScale, punchScale, elapsed / (attackDuration / 2f));
            yield return null;
        }

        elapsed = 0f;
        // Punch back
        while (elapsed < attackDuration / 2f)
        {
            elapsed += Time.deltaTime;
            modelTransform.localScale = Vector3.Lerp(punchScale, modelBaseScale, elapsed / (attackDuration / 2f));
            yield return null;
        }

        modelTransform.localScale = modelBaseScale;
        isAttacking = false;
    }

    public void PlayDeathJuice()
    {
        if (isDead) return;
        isDead = true;
        StartCoroutine(DeathTipRoutine());
    }

    private System.Collections.IEnumerator DeathTipRoutine()
    {
        float elapsed = 0f;
        Quaternion startRot = modelTransform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0f, 0f, 90f);

        while (elapsed < deathTipDuration)
        {
            elapsed += Time.deltaTime;
            modelTransform.rotation = Quaternion.Slerp(startRot, endRot, elapsed / deathTipDuration);
            yield return null;
        }
    }

    public bool IsDead => isDead;
}
