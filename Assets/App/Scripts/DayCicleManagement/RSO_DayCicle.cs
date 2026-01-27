using UnityEngine;
using MVsToolkit.Wrappers;

[CreateAssetMenu(fileName = "RSO_DayCicle", menuName = "RSO/Management/RSO_DayCicle")]
public class RSO_DayCicle : RuntimeScriptableObject<DayCicleState>{}

public enum DayCicleState
{
    Day,
    Evening,
    Night
}