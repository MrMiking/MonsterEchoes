using UnityEngine;
using System.Collections;
using MVsToolkit.Dev;

public class BossAttack_3 : BossPatern
{
    [Header("Settings")]
    [SerializeField] float distanceRequire;
    [SerializeField] float timeToTp;
    [SerializeField] int damage;

    [Foldout("Attack")]
    [SerializeField] Vector2 attack1Offset;
    [SerializeField] float attack1Radius;

    [Space(5)]
    [SerializeField] Vector2 startAttack2Pos;
    [SerializeField] Vector2 endAttack2Pos;
    
    [Space(5)]
    [SerializeField] Vector2 startAttack3Pos;
    [SerializeField] Vector2 endAttack3Pos;
    [CloseFoldout]

    [Header("References")]
    [SerializeField] BossVisual visual;
    [SerializeField] BossMovement movement;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] RSO_Player player;

    //[Header("References")]
    //[Header("Input")]
    //[Header("Output")]

    public override bool CanHandle()
    {
        return canHandle
            && Vector2.Distance(transform.position, player.Get().transform.position) <= distanceRequire;
    }

    public override IEnumerator Handle()
    {
        base.Handle();

        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;

        visual.Attack3();
        StartCoroutine(TpTime());
        yield return new WaitForSeconds(paternTime);

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
                playerHealth.TakeDamage(damage);
            }
        }
    }

    public void ApplyAttack2()
    {
        RaycastHit2D[] hits = Physics2D.LinecastAll(startAttack2Pos, endAttack2Pos);

        foreach (RaycastHit2D hit in hits)
        {
            if(hit.collider.TryGetComponent(out PlayerHealth playerHealth))
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    public void ApplyAttack3()
    {
        RaycastHit2D[] hits = Physics2D.LinecastAll(startAttack3Pos, endAttack3Pos);

        foreach (RaycastHit2D hit in hits)
        {
            if(hit.collider.TryGetComponent(out PlayerHealth playerHealth))
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