using MVsToolkit.Dev;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BossController bossPrefab;
    [SerializeField] private Transform bossSpawnPoint;

    [Header("Input")]
    [SerializeField] private RSO_DayCycle dayCycle;

    private GameObject currentBoss;

    private void OnEnable()
    {
        dayCycle.OnChanged += HandleDayCycleChange;
    }

    private void OnDisable()
    {
        dayCycle.OnChanged -= HandleDayCycleChange;
    }

    private void HandleDayCycleChange(DayCycleState newCycle)
    {
        if (newCycle == DayCycleState.Night)
        {
            SpawnBoss();
        }
    }

    [Button]
    private void SpawnBoss()
    {
        if (bossPrefab != null && bossSpawnPoint != null)
        {
            currentBoss = Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation).gameObject;
        }
    }

}