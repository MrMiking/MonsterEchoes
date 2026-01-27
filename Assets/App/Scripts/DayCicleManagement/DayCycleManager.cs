using Unity.VisualScripting;
using UnityEngine;

public class DayCycleManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] DayCycleState startingCycle;

    [Header("References")]
    [SerializeField] RSO_DayCount dayCount;
    [SerializeField] RSO_DayCycle currentCycle;

    public static DayCycleManager instance;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        currentCycle.Set(startingCycle);
        dayCount.Set(1);
    }

    public void HandleNextCycle()
    {
        switch (currentCycle.Get())
        {
            case DayCycleState.Day:
                currentCycle.Set(DayCycleState.Evening);
                break;
            
            case DayCycleState.Evening:
                currentCycle.Set(DayCycleState.Night);
                break;
            
            case DayCycleState.Night:
                dayCount.Set(dayCount.Get() + 1);
                currentCycle.Set(DayCycleState.Day);
                break;
        }
    }
}