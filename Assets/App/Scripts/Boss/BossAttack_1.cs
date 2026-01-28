using System.Collections;
using UnityEngine;

public class BossAttack_1 : BossPatern
{
    [Header("Settings")]
    [SerializeField] RSO_BossDamage damage;
    [SerializeField] float distanceRequire;

    [Space(10)]
    [SerializeField] Vector2 detectionOffset;
    [SerializeField] Vector2 detectionSize;

    [Header("References")]
    [SerializeField] RSO_Player player;
    BossVisual visual;

    //[Header("Input")]
    //[Header("Output")]

    public override void Init(BossController controller)
    {
        visual = controller.visual;
        visual.OnAttack1Dmg += ApplyDamage;
    }

    public override bool CanHandle()
    {
        return canHandle 
            && Vector2.Distance(transform.position, player.Get().transform.position) <= distanceRequire;
    }

    public override IEnumerator Handle()
    {
        StartCoroutine(HandleCooldown());
        visual.Attack1();
        yield return new WaitForSeconds(paternTime * paternTimeMult.Value);
    }

    public void ApplyDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCapsuleAll(
            (Vector2)transform.position + detectionOffset,
            detectionSize,
            detectionSize.y > detectionSize.x ? CapsuleDirection2D.Vertical : CapsuleDirection2D.Horizontal,
            0);

        MVsDebug.Draw2DCapsule(
            (Vector2)transform.position + detectionOffset,
            detectionSize, Color.blue, 1);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out PlayerHealth health))
            {
                health.TakeDamage(damage.Get());
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceRequire);

        if (Application.isPlaying) return;
        MVsGizmos.Draw2DCapsule((Vector2)transform.position + detectionOffset, detectionSize, Color.blue);
    }
}