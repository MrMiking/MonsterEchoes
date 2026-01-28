using UnityEngine;
using MVsToolkit.Wrappers;

[CreateAssetMenu(fileName = "RSO_BossHealth", menuName = "RSO/Boss/RSO_BossHealth")]
public class RSO_BossHealth : RuntimeScriptableObject<float>
{
    public void AddHealth(float healthGiven)
    {
        Value += healthGiven;
    }

    public void RemoveHealth(float healthGiven)
    {
        Value -= healthGiven;

        if(Value < 1) Value = 1;
    }
}