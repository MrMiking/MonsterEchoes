using UnityEngine;
using UnityEngine.Events;

public class BossHealth : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] int maxHealth;

    [Header("References")]
    [SerializeField] BossVisual visual;
    [SerializeField] RSO_BossHealth currentHealth;

    //[Header("Input")]
    [Header("Output")]
    [SerializeField] UnityEvent OnTakeDamage;
    [SerializeField] UnityEvent OnDeath;
    [SerializeField] RSE_OnBossMidLife onMidLife;

    public int GetMaxHealth { get { return maxHealth; } }

    private void Start()
    {
        currentHealth.Set(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        float lastHealth = currentHealth.Value;
        currentHealth.Set(currentHealth.Get() - damage);
        if(lastHealth > maxHealth * .5f && currentHealth.Value < maxHealth * .5f)
        {
            onMidLife.Call();
        }

        if (currentHealth.Get() <= 0)
        {
            Die();
            return;
        }

        OnTakeDamage?.Invoke();
    }

    public void Die()
    {
        OnDeath?.Invoke();
    }
}