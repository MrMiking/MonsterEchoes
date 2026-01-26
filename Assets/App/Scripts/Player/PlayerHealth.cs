using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private RSO_Health rsoHealth;
    
    private float currentHealth;
    
    public float GetCurrentHealth { get { return currentHealth; } }
    public float GetMaxHealth { get { return health; } }

    private void Start()
    {
        currentHealth = health;
        rsoHealth.Set(currentHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        
        rsoHealth.Set(health);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player Is Dead");
    }
}
