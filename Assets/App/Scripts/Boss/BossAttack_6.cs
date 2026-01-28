using UnityEngine;
using System.Collections;

public class BossAttack_6 : BossPatern
{
    [Header("Settings")]
    [SerializeField] float groundPos;
    [SerializeField] float spacing;
    [SerializeField] float startXOffset;
    [SerializeField] float delayBetween;

    [Header("References")]
    [SerializeField] FXCircleDamage[] bombsL;
    [SerializeField] FXCircleDamage[] bombsR;
    [SerializeField] RSO_Player player;

    //[Header("Input")]
    //[Header("Output")]

    public override IEnumerator Handle()
    {
        StartCoroutine(HandleCooldown());
        StartCoroutine(SpawnBombs(bombsL, Vector2.left, false));
        StartCoroutine(SpawnBombs(bombsR, Vector2.right, true));

        yield return new WaitForSeconds(paternTime);
    }

    IEnumerator SpawnBombs(FXCircleDamage[] bombs, Vector2 dir, bool isRight)
    {
        Vector2 startOffset = (Vector2)transform.position + dir * startXOffset;
        for (int i = 0; i < bombs.Length; i++)
        {
            bombs[i].Flip(isRight);
            bombs[i].transform.position = startOffset + dir * spacing * i;
            bombs[i].gameObject.SetActive(true);
            bombs[i].PlayAnim();

            yield return new WaitForSeconds(delayBetween);
        }
    }
}