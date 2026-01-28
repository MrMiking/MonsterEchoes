using UnityEngine;
using System.Collections;
using MVsToolkit.Dev;

public class BossAttack_3 : BossPatern
{
    [Header("Settings")]
    [SerializeField] float distanceRequire;
    [SerializeField] float timeToTp;
    [SerializeField] RSO_BossDamage damage;

    [Foldout("Attack")]
    [SerializeField] Vector2 attack1Offset;
    [SerializeField] float attack1Radius;
    [SerializeField] float knockbackForce;

    [Space(5)]
    [SerializeField] Vector2 startAttack2Pos;
    [SerializeField] Vector2 endAttack2Pos;
    
    [Space(5)]
    [SerializeField] Vector2 startAttack3Pos;
    [SerializeField] Vector2 endAttack3Pos;
    [CloseFoldout]

    [Header("References")]
    BossVisual visual;
    BossMovement movement;
    Rigidbody2D rb;
    [SerializeField] RSO_Player player;

    //[Header("References")]
    //[Header("Input")]
    //[Header("Output")]

    public override void Init(BossController controller)
    {
        visual = controller.visual;
        movement = controller.movement;
        rb = controller.rb;
        visual.OnAttack3Dmg1 += ApplyAttack1;
        visual.OnAttack3Dmg2 += ApplyAttack2;
        visual.OnAttack3Dmg3 += ApplyAttack3;
    }

    public override bool CanHandle()
    {
        return canHandle
            && Vector2.Distance(transform.position, player.Get().transform.position) <= distanceRequire;
    }

    public override IEnumerator Handle()
    {
        StartCoroutine(HandleCooldown());
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;

        visual.Attack3();
        StartCoroutine(TpTime());
        yield return new WaitForSeconds(paternTime * paternTimeMult.Value);

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    IEnumerator TpTime()
    {
        yield return new WaitForSeconds(timeToTp);

        Vector2 target = player.Get().transform.position;
        target.y = rb.position.y;

        movement.TpToo(target);
    }

    public void ApplyAttack1()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            (Vector2)transform.position + attack1Offset,
            attack1Radius);

        foreach (Collider2D hit in hits)
        {
            if(hit.TryGetComponent(out PlayerHealth playerHealth))
            {
                playerHealth.TakeDamage(damage.Get());

                if(hit.TryGetComponent(out PlayerController player) && !player.Health.isInvincible)
                {
                    Vector2 dir = Vector2.up;
                    if (transform.position.x > player.transform.position.x) dir.x = 2;
                    else dir.x = -2;
                    dir = dir.normalized;

                    player.rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }
    }

    public void ApplyAttack2()
    {
        RayLine((Vector2)transform.position + startAttack2Pos,
            (Vector2)transform.position + endAttack2Pos);

        Vector2 leftUp = new Vector2(-1, 1);
        RayLine((Vector2)transform.position + startAttack2Pos * leftUp,
            (Vector2)transform.position + endAttack2Pos * leftUp);
    }

    public void ApplyAttack3()
    {
        RayLine((Vector2)transform.position + startAttack3Pos,
            (Vector2)transform.position + endAttack3Pos);

        Vector2 leftUp = new Vector2(-1, 1);
        RayLine((Vector2)transform.position + startAttack3Pos * leftUp,
            (Vector2)transform.position + endAttack3Pos * leftUp);
    }

    void RayLine(Vector2 pos1, Vector2 pos2)
    {
        RaycastHit2D[] hits = Physics2D.LinecastAll(pos1, pos2);
        Debug.DrawLine(pos1, pos2, Color.blue, 1);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.TryGetComponent(out PlayerHealth playerHealth))
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceRequire);

        Gizmos.color = Color.orange;
        Gizmos.DrawWireSphere((Vector2)transform.position + attack1Offset, attack1Radius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine((Vector2)transform.position + startAttack2Pos, (Vector2)transform.position + endAttack2Pos);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine((Vector2)transform.position + startAttack3Pos, (Vector2)transform.position + endAttack3Pos);

        Vector2 leftUp = new Vector2(-1, 1);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine((Vector2)transform.position + startAttack2Pos * leftUp, (Vector2)transform.position + endAttack2Pos * leftUp);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine((Vector2)transform.position + startAttack3Pos * leftUp, (Vector2)transform.position + endAttack3Pos * leftUp);
    }
}