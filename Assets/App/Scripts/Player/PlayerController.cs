using MVsToolkit.Dev;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage);
}

public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("References")]
    public PlayerMovement Movement;
    public PlayerVisual Visual;
    public PlayerCombat Combat;
    public PlayerHealth Health;
    public Rigidbody2D rb;

    [Header("RSO")]
    [SerializeField] RSO_Player player;

    private void Awake()
    {
        player.Set(this);
    }

    public void TakeDamage(float damage)
    {
        Health.TakeDamage(damage);
    }
}