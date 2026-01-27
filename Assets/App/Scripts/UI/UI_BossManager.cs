using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RSO_Boss boss;
    [SerializeField] RSO_DayCycle currentCycle;

    [Header("Health")]
    [SerializeField] private float animDuration = 0.2f;
    [SerializeField] private RSO_BossHealth rsoBossHealth;
    [SerializeField] private Slider bossHealthSlider;

    private void OnEnable()
    {
        rsoBossHealth.OnChanged += UpdateHealthBar;
        currentCycle.OnChanged += HandleDayCycleChange;
    }

    private void OnDisable()
    {
        rsoBossHealth.OnChanged -= UpdateHealthBar;
        currentCycle.OnChanged -= HandleDayCycleChange;
    }

    private void UpdateHealthBar(float newValue)
    {
        Debug.Log("Health: " + newValue);
        bossHealthSlider.maxValue = boss.Get().Health.GetMaxHealth;

        bossHealthSlider.DOValue(newValue, animDuration).SetEase(Ease.OutCubic);
    }

    private void HandleDayCycleChange(DayCycleState newCycle)
    {
        if (newCycle == DayCycleState.Night)
        {
            bossHealthSlider.gameObject.SetActive(true);
        }
        else
        {
            bossHealthSlider.gameObject.SetActive(false);
        }
    }
}