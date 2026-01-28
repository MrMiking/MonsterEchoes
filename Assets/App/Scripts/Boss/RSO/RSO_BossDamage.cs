using UnityEngine;
using MVsToolkit.Wrappers;

[CreateAssetMenu(fileName = "RSO_BossDamage", menuName = "RSO/Boss/RSO_BossDamage")]
public class RSO_BossDamage : RuntimeScriptableObject<int>
{
    public void AddDamage(int damageGiven)
    {
        Value += damageGiven;
    }

    public void RemoveDamage(int damageGiven)
    {
        Value -= damageGiven;

        if(Value < 1) Value = 1;
    }
}