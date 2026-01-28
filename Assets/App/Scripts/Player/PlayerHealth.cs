using MVsToolkit.Dev;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private RSO_Health currentHealth;

    [Space(5)]
    [ReadOnly] public bool isInvincible = false;
    bool isDead = false;

    [Header("References")]
    [SerializeField] Animator anim;

    [Header("Output")]
    [SerializeField] private UnityEvent OnTakeDamage;
    [SerializeField] private UnityEvent OnDeath;

    public float GetMaxHealth { get { return maxHealth; } }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        currentHealth.Set(maxHealth);
        isDead = false;
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible || isDead) return;

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
        anim.SetTrigger("Die");
        anim.SetBool("IsDead", true);
        isDead = true;
        UIContextManager.Instance.PushContext(UIContext.Menu);

        OnDeath?.Invoke();
    }
}