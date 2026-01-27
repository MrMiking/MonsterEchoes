using MVsToolkit.Dev;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private RSO_Health currentHealth;

    [Space(5)]
    [ReadOnly] public bool isInvincible = false;

    [Header("Output")]
    [SerializeField] private UnityEvent OnTakeDamage;
    [SerializeField] private UnityEvent OnDeath;

    public float GetMaxHealth { get { return maxHealth; } }

    private void Start()
    {
        currentHealth.Set(maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible) return;

        CameraController.Instance?.Shake(12, .3f);
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