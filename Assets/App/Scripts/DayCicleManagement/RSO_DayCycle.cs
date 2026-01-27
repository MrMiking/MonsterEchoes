using UnityEngine;
using MVsToolkit.Wrappers;

[CreateAssetMenu(fileName = "RSO_DayCycle", menuName = "RSO/Management/RSO_DayCycle")]
public class RSO_DayCycle : RuntimeScriptableObject<DayCycleState>{}

public enum DayCycleState
{
    Day,
    Evening,
    Night
}