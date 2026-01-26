using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using MVsToolkit.Utils;

public class BossController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float checkingDelay;
    [SerializeField] List<BossPatern> paterns;

    [Header("References")]
    [SerializeField] BossMovement movement;
    [SerializeField] BossVisual visual;

    [SerializeField] RSO_Player player;

    //[Header("Input")]
    //[Header("Output")]

    private void Start()
    {
        StartCoroutine(CheckPatern());
    }

    IEnumerator CheckPatern(bool moveWhilWaiting = false)
    {
        if (moveWhilWaiting)
        {
            float t = 0;
            while(t < checkingDelay)
            {
                yield return new WaitForFixedUpdate();

                Vector2 dir = (player.Get().transform.position - transform.position).normalized;
                dir.y = 0;

                movement.Move(dir);
                t += Time.fixedDeltaTime;
            }
        }
        else yield return new WaitForSeconds(checkingDelay);

        BossPatern[] p = paterns.FindAll(c => c.CanHandle()).ToArray();

        if(p.Length == 0)
        {
            StartCoroutine(CheckPatern(true));
            yield break;
        }
        visual.SetMoveXInput(0);

        yield return StartCoroutine(p.GetRandom().Handle());

        StartCoroutine(CheckPatern());
    }
}