using UnityEngine;

public class FXCircleDamage : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] RSO_BossDamage damage;

    [SerializeField] Vector2 detectionOffset = Vector2.right;
    Vector2 _detectionOffset;

    [SerializeField] float detectionRadius;

    [Header("References")]
    [SerializeField] SpriteRenderer graphics;
    [SerializeField] Animator anim;

    public void PlayAnim()
    {
        anim.SetTrigger("Play");
    }

    public void Flip(bool isRight)
    {
        graphics.flipX = isRight;

        _detectionOffset = detectionOffset;
        _detectionOffset.x *= isRight ? -1 : 1;
    }

    public void ApplyDamage()
    {
        Vector2 center = (Vector2)transform.position + _detectionOffset;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            center,
            detectionRadius
        );

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out PlayerHealth health))
            {
                health.TakeDamage(damage.Get());
            }
        }
    }

    public void OnAnimComplete()
    {
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Vector2 center = (Vector2)transform.position + detectionOffset;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(center, detectionRadius);
    }
}