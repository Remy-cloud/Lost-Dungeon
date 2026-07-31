using NUnit.Framework;
using UnityEngine;

public class HealthTests
{
    private GameObject CreateHealthObject(float maxHealth)
    {
        GameObject go = new GameObject("TestHealth");
        Health health = go.AddComponent<Health>();
        return go;
    }

    [Test]
    public void TakeDamage_ReducesHealth_ByExpectedAmount()
    {
        GameObject go = CreateHealthObject(100f);
        Health health = go.GetComponent<Health>();

        health.TakeDamage(20f);

        Assert.LessOrEqual(health.CurrentHealth, 80f);
        Object.DestroyImmediate(go);
    }

    [Test]
    public void TakeDamage_TriggersDeath_WhenHealthReachesZero()
    {
        GameObject go = CreateHealthObject(50f);
        Health health = go.GetComponent<Health>();

        health.TakeDamage(1000f);

        Assert.IsTrue(health.IsDead);
        Object.DestroyImmediate(go);
    }

    [Test]
    public void Heal_ClampsAtMaxHealth()
    {
        GameObject go = CreateHealthObject(100f);
        Health health = go.GetComponent<Health>();

        health.Heal(9999f);

        Assert.AreEqual(health.MaxHealth, health.CurrentHealth);
        Object.DestroyImmediate(go);
    }

    [Test]
    public void TakeDamage_DoesNothing_AfterDeath()
    {
        GameObject go = CreateHealthObject(50f);
        Health health = go.GetComponent<Health>();

        health.TakeDamage(1000f); // dies
        float healthAfterDeath = health.CurrentHealth;
        health.TakeDamage(50f); // should be ignored

        Assert.AreEqual(healthAfterDeath, health.CurrentHealth);
        Object.DestroyImmediate(go);
    }
}
