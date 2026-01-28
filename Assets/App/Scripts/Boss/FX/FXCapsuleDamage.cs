using UnityEngine;

public class FXCapsuleDamage : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] RSO_BossDamage damage;

    [SerializeField] Vector2 detectionOffset = Vector2.right;
    Vector2 _detectionOffset;
    [SerializeField] Vector2 detectionSize;

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

        Collider2D[] hits = Physics2D.OverlapCapsuleAll(
            center,
            detectionSize,
            detectionSize.y > detectionSize.x ? CapsuleDirection2D.Vertical : CapsuleDirection2D.Horizontal,
            0f
        );

        MVsDebug.Draw2DCapsule(center, detectionSize, Color.blue, 1);

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
        if (Application.isPlaying) return;
        Vector2 center = (Vector2)transform.position + detectionOffset;
        MVsGizmos.Draw2DCapsule(center, detectionSize, Color.blue);
    }
}