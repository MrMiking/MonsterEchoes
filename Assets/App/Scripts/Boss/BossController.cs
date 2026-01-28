using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using MVsToolkit.Utilities;

public class BossController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float checkingDelay;
    [SerializeField] List<BossPatern> paterns;
    [SerializeField] int startingDamage;

    [Header("References")]
    public BossMovement movement;
    public BossVisual visual;
    public BossHealth Health;
    public Rigidbody2D rb;

    [SerializeField] RSO_Boss boss;
    [SerializeField] RSO_Player player;
    [SerializeField] RSO_BossDamage damage;

    private void Awake()
    {
        boss.Set(this);
    }

    private void Start()
    {
        if(damage.Value == 0) damage.Value = startingDamage;
        StartCoroutine(CheckPatern());

        foreach (var patern in paterns)
        {
            patern.Init(this);
        }
    }

    public void AddPatern(BossPatern patern)
    {
        BossPatern _patern = Instantiate(patern, transform);
        _patern.Init(this);
        paterns.Add(_patern);
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