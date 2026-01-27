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

    public int GetMaxHealth { get { return maxHealth; } }

    private void Start()
    {
        currentHealth.Set(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log(damage);
        currentHealth.Set(currentHealth.Get() - damage);

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