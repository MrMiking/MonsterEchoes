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

    void DebugDrawCapsule(Vector2 center, Vector2 size, Color color, float duration = 0f)
    {
        float radius = Mathf.Min(size.x, size.y) * 0.5f;
        float height = Mathf.Max(size.x, size.y);
        float cylinderLength = height - radius * 2f;

        bool vertical = size.y > size.x;

        if (vertical)
        {
            Vector2 top = center + Vector2.up * (cylinderLength * 0.5f);
            Vector2 bottom = center + Vector2.down * (cylinderLength * 0.5f);

            // Lignes verticales
            Debug.DrawLine(top + Vector2.left * radius, bottom + Vector2.left * radius, color, duration);
            Debug.DrawLine(top + Vector2.right * radius, bottom + Vector2.right * radius, color, duration);

            // Demi-cercles
            DebugDrawCircle(top, radius, color, duration);
            DebugDrawCircle(bottom, radius, color, duration);
        }
        else
        {
            Vector2 right = center + Vector2.right * (cylinderLength * 0.5f);
            Vector2 left = center + Vector2.left * (cylinderLength * 0.5f);

            Debug.DrawLine(left + Vector2.up * radius, right + Vector2.up * radius, color, duration);
            Debug.DrawLine(left + Vector2.down * radius, right + Vector2.down * radius, color, duration);

            DebugDrawCircle(left, radius, color, duration);
            DebugDrawCircle(right, radius, color, duration);
        }
    }

    void DebugDrawCircle(Vector2 center, float radius, Color color, float duration = 0f, int segments = 24)
    {
        float step = 360f / segments;
        Vector3 prev = center + new Vector2(Mathf.Cos(0), Mathf.Sin(0)) * radius;

        for (int i = 1; i <= segments; i++)
        {
            float rad = Mathf.Deg2Rad * (i * step);
            Vector3 next = center + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;
            Debug.DrawLine(prev, next, color, duration);
            prev = next;
        }
    }
}