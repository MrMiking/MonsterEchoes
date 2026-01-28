using UnityEngine;
using System.Collections;

public class BossAttack_5 : BossPatern
{
    [Header("Settings")]
    [SerializeField] float groundPos;
    [SerializeField] float startXOffset;

    [Header("References")]
    [SerializeField] FXCapsuleDamage reaper;
    [SerializeField] RSO_Player player;

    //[Header("Input")]
    //[Header("Output")]

    public override IEnumerator Handle()
    {
        StartCoroutine(HandleCooldown());

        bool isRight = transform.position.x > player.Value.transform.position.x;
        reaper.Flip(isRight);
        reaper.transform.position = new Vector2(
            player.Value.transform.position.x + (isRight ? -1 : 1) * startXOffset, 
            groundPos);
        reaper.gameObject.SetActive(true);
        reaper.PlayAnim();

        yield return new WaitForSeconds(paternTime);
    }
}
