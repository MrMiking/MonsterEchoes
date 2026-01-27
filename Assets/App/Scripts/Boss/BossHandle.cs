using System.Collections;
using MVsToolkit.Dev;
using UnityEngine;

public class BossPatern : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected float cooldown;
    [SerializeField] protected float paternTime;

    [ReadOnly] public bool canHandle = true;
    [ReadOnly] public bool isHandling = false;

    public virtual IEnumerator Handle() 
    {
        yield break;
    }
    public virtual bool CanHandle() { return canHandle; }

    protected IEnumerator HandleCooldown()
    {
        canHandle = false;
        yield return new WaitForSeconds(cooldown);
        canHandle = true;
    }
}