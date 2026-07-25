using UnityEngine;
using System;
using System.Collections;

public class Health : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("Hit Flash Settings")]
    [SerializeField] private Renderer[] renderersToFlash; // leave empty to auto-find
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.15f;

    public static event Action<Health, float, float> OnHealthChanged; // (who, current, max)
    public static event Action<Health> OnDeath;

    public bool IsDead { get; private set; } = false;

    private Color[] originalColors;

    void Awake()
    {
        currentHealth = maxHealth;

        if (renderersToFlash == null || renderersToFlash.Length == 0)
            renderersToFlash = GetComponentsInChildren<Renderer>();

        originalColors = new Color[renderersToFlash.Length];
        for (int i = 0; i < renderersToFlash.Length; i++)
        {
            if (renderersToFlash[i].material.HasProperty("_Color"))
                originalColors[i] = renderersToFlash[i].material.color;
        }
    }

    public void TakeDamage(float amount)
    {
        if (IsDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        OnHealthChanged?.Invoke(this, currentHealth, maxHealth);
        StartCoroutine(FlashRoutine());

        if (currentHealth <= 0f && !IsDead)
        {
            Die();
        }
    }

    private IEnumerator FlashRoutine()
    {
        foreach (Renderer r in renderersToFlash)
        {
            if (r != null && r.material.HasProperty("_Color"))
                r.material.color = flashColor;
        }

        yield return new WaitForSeconds(flashDuration);

        for (int i = 0; i < renderersToFlash.Length; i++)
        {
            if (renderersToFlash[i] != null && renderersToFlash[i].material.HasProperty("_Color"))
                renderersToFlash[i].material.color = originalColors[i];
        }
    }

    public void Heal(float amount)
    {
        if (IsDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        OnHealthChanged?.Invoke(this, currentHealth, maxHealth);
    }

    private void Die()
    {
        IsDead = true;
        OnDeath?.Invoke(this);
    }

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
}
