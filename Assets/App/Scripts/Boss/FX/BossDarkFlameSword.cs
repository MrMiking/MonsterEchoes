using UnityEngine;

public class BossDarkFlameSword : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] RSO_BossDamage damage;

    [SerializeField] Vector2 detectionOffset;
    [SerializeField] Vector2 detectionSize;

    //[Header("References")]
    //[Header("Input")]
    //[Header("Output")]

    public void ApplyDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCapsuleAll(
    (Vector2)transform.position + detectionOffset,
    detectionSize,
    detectionSize.y > detectionSize.x ? CapsuleDirection2D.Vertical : CapsuleDirection2D.Horizontal,
    0);

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
        MVsGizmos.Draw2DCapsule((Vector2)transform.position + detectionOffset, detectionSize, Color.blue);
    }
}