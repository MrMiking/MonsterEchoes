using UnityEngine;

public class FXCapsuleDamage : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] RSO_BossDamage damage;

    [SerializeField] Vector2 detectionOffset = Vector2.right;
    [SerializeField] Vector2 detectionSize;

    [Header("References")]
    [SerializeField] SpriteRenderer graphics;
    [SerializeField] Animator anim;

    // Offset de base (toujours configuré vers la droite dans l’inspecteur)
    Vector2 baseDetectionOffset;
    int facingSign = 1; // 1 = droite, -1 = gauche

    void Awake()
    {
        baseDetectionOffset = detectionOffset;
    }

    public void PlayAnim()
    {
        anim.SetTrigger("Play");
    }

    public void Flip(bool isRight)
    {
        graphics.flipX = isRight;
        facingSign = isRight ? -1 : 1;

        // On applique le flip uniquement sur l’axe X
        detectionOffset = new Vector2(baseDetectionOffset.x * facingSign, baseDetectionOffset.y);
    }

    public void ApplyDamage()
    {
        Vector2 center = (Vector2)transform.position + detectionOffset;

        Collider2D[] hits = Physics2D.OverlapCapsuleAll(
            center,
            detectionSize,
            detectionSize.y > detectionSize.x ? CapsuleDirection2D.Vertical : CapsuleDirection2D.Horizontal,
            0f
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
        // En mode édition, on essaie de garder le même comportement
        Vector2 center = (Vector2)transform.position + detectionOffset;
        MVsGizmos.Draw2DCapsule(center, detectionSize, Color.blue);
    }
}