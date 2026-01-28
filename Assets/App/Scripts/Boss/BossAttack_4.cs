using System.Collections;
using UnityEngine;

public class BossAttack_4 : BossPatern
{
    [Header("Settings")]
    [SerializeField] float groundPos;
    [SerializeField] float spacing;
    [SerializeField] float startXOffset;
    [SerializeField] float delayBetween;

    [Header("References")]
    [SerializeField] BossDarkFlameSword[] flames;
    [SerializeField] RSO_Player player;

    //[Header("Input")]
    //[Header("Output")]

    public override IEnumerator Handle()
    {
        StartCoroutine(HandleCooldown());

        bool isRight = transform.position.x > player.Value.transform.position.x;
        Vector2 startOffset = (Vector2)transform.position + (isRight ? Vector2.left : Vector2.right) * startXOffset;
        for (int i = 0; i < flames.Length; i++)
        {
            flames[i].Flip(isRight);
            flames[i].transform.position = startOffset + (isRight ? Vector2.left : Vector2.right) * spacing * i;
            flames[i].gameObject.SetActive(true);
            flames[i].PlayAnim();

            yield return new WaitForSeconds(delayBetween);
        }

        yield return new WaitForSeconds(paternTime);
    }
}