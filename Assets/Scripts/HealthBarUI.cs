using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Transform playerTransform; // to check "is this event about the player"

    void OnEnable()
    {
        Health.OnHealthChanged += UpdateHealthBar;
    }

    void OnDisable()
    {
        Health.OnHealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(Health health, float current, float max)
    {
        if (health.gameObject != playerTransform.gameObject) return; // only react to the player's health, not enemies

        healthSlider.maxValue = max;
        healthSlider.value = current;
    }
}
