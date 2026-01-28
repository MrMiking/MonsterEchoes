using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using MVsToolkit.Utilities;

public class BossController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float checkingDelay;
    [SerializeField] List<BossPatern> paterns;
    [SerializeField] List<BossPatern> p2Paterns;

    [Header("References")]
    public BossMovement movement;
    public BossVisual visual;
    public BossHealth Health;
    public Rigidbody2D rb;

    [SerializeField] RSO_Boss boss;
    [SerializeField] RSO_Player player;

    [Header("Input")]
    [SerializeField] RSE_OnBossMidLife OnMidLife;

    private void OnEnable()
    {
        OnMidLife.Action += OnP2;
    }
    private void OnDisable()
    {
        OnMidLife.Action -= OnP2;
    }

    private void Awake()
    {
        boss.Set(this);
    }

    private void Start()
    {
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

    public void OnP2()
    {
        foreach (var patern in p2Paterns)
        {
            AddPatern(patern);
        }
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