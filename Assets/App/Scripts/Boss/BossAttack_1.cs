using System.Collections;
using UnityEngine;

public class BossAttack_1 : BossPatern
{
    [Header("Settings")]
    [SerializeField] float distanceRequire;

    [Header("References")]
    [SerializeField] BossVisual visual;
    [SerializeField] RSO_Player player;

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

        visual.Attack1();
        yield return new WaitForSeconds(paternTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceRequire);
    }
}