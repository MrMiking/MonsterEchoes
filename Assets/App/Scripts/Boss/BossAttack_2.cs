using System.Collections;
using UnityEngine;

public class BossAttack_2 : BossPatern
{
    [Header("Settings")]
    [SerializeField] float moveSpeed;
    [SerializeField] float maxDistance;

    [Header("References")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] BossVisual visual;

    [Space(5)]
    [SerializeField] RSO_Player player;

    //[Header("Input")]
    //[Header("Output")]

    public override bool CanHandle()
    {
        return canHandle
            && Vector2.Distance(transform.position, player.Get().transform.position) >= maxDistance;
    }

    public override IEnumerator Handle()
    {
        base.Handle();

        float t = 0;
        Vector2 dir = (player.Get().transform.position - transform.position).normalized;

        visual.FlipX(dir.x);
        visual.Attack2();

        while (t < paternTime)
        {
            yield return new WaitForFixedUpdate();
            t += Time.fixedDeltaTime;

            rb.AddForce(dir * moveSpeed);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}