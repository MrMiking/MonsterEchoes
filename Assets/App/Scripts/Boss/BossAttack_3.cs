using UnityEngine;
using System.Collections;

public class BossAttack_3 : BossPatern
{
    [Header("Settings")]
    [SerializeField] float distanceRequire;
    [SerializeField] float timeToTp;

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceRequire);
    }
}