using UnityEngine;

[System.Serializable]
public class PlayerAttack
{
    [Header("References")]
    public float AttackTime;
    public int damage;

    public Vector2 pos;
    public Vector2 size;
    public Color debugColor;

    [Space(5)]
    [SerializeField] float attackDashForce;

    [Header("References")]
    [SerializeField] Rigidbody2D rb;

    public virtual void Attack(Vector2 playerPos, bool right = true) 
    {
        rb.AddForce((right ? Vector2.right : Vector2.left) * attackDashForce, ForceMode2D.Impulse);

        Vector2 posOffset = pos;
        posOffset.x *= (right ? 1 : -1);

        Collider2D[] hits = Physics2D.OverlapCapsuleAll(
            playerPos + posOffset,
            size,
            size.y > size.x ? CapsuleDirection2D.Vertical : CapsuleDirection2D.Horizontal,
            0);

        foreach (Collider2D hit in hits)
        {
            if(hit.TryGetComponent(out BossHealth health))
            {
                health.TakeDamage(damage);
            }
        }
    }
}