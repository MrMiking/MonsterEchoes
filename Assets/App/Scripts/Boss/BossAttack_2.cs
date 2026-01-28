using System.Collections;
using UnityEngine;

public class BossAttack_2 : BossPatern
{
    [Header("Settings")]
    [SerializeField] float moveSpeed;
    [SerializeField] float maxDistance;
    [SerializeField] float timeBeforeStop;

    [Space(10)]
    [SerializeField] Vector2 detectionSize;
    [SerializeField] Vector2 detectionOffset;
    [SerializeField] RSO_BossDamage damage;

    Rigidbody2D rb;
    BossVisual visual;

    [Space(5)]
    [SerializeField] RSO_Player player;

    //[Header("Input")]
    //[Header("Output")]

    public override void Init(BossController controller)
    {
        rb = controller.rb;
        visual = controller.visual;
        visual.OnAttack2Dmg1 += ApplyDamage;
        visual.OnAttack2Dmg2 += ApplyDamage;
    }

    public override bool CanHandle()
    {
        return canHandle
            && Vector2.Distance(transform.position, player.Get().transform.position) >= maxDistance;
    }

    public override IEnumerator Handle()
    {
        StartCoroutine(HandleCooldown());
        float t = 0;
        Vector2 dir = (player.Get().transform.position - transform.position).normalized;
        dir.y = 0;

        visual.FlipX(dir.x);
        visual.Attack2();

        while (t < paternTime * paternTimeMult.Value - timeBeforeStop)
        {
            yield return new WaitForFixedUpdate();
            t += Time.fixedDeltaTime;

            rb.AddForce(dir * moveSpeed);
        }

        yield return new WaitForSeconds(timeBeforeStop);
    }

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

        MVsDebug.Draw2DCapsule((Vector2)transform.position + detectionOffset,
            detectionSize, Color.blue, 1);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxDistance);

        MVsGizmos.Draw2DCapsule((Vector2)transform.position + detectionOffset, detectionSize, Color.blue);
    }
}