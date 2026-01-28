using UnityEngine;
using MVsToolkit.Wrappers;

[CreateAssetMenu(fileName = "RSO_BossPaternTimeMultiplier", menuName = "RSO/_/RSO_BossPaternTimeMultiplier")]
public class RSO_BossPaternTimeMultiplier : RuntimeScriptableObject<float>
{
    public void AddMultiplier(float multiplier)
    {
        Value += multiplier;
    }

    public void RemoveMultiplier(float multiplier)
    {
        Value -= multiplier;
    }
}