using UnityEngine;

public class DayCicleManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] DayCicleState startingCycle;

    [Header("References")]
    [SerializeField] RSO_DayCicle currentCicle;

    //[Header("Input")]
    //[Header("Output")]

    private void Start()
    {
        currentCicle.Set(startingCycle);
    }

    public void HandleNextCycle()
    {
        switch (currentCicle.Get())
        {
            case DayCicleState.Day:
                currentCicle.Set(DayCicleState.Evening);
                break;
            
            case DayCicleState.Evening:
                currentCicle.Set(DayCicleState.Night);
                break;
            
            case DayCicleState.Night:
                currentCicle.Set(DayCicleState.Day);
                break;
        }
    }
}