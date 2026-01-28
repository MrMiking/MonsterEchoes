using DG.Tweening;
using MVsToolkit.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RSO_Boss rsoBoss;
    [SerializeField] RSO_DayCycle currentCycle;

    [Header("Health")]
    [SerializeField] private float delayBeforeAnim = 0.5f;
    [SerializeField] private float animDuration = 0.5f;
    [SerializeField] private RSO_BossHealth rsoBossHealth;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider innerHealthSlider;

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
        innerHealthSlider.maxValue = rsoBoss.Get().Health.GetMaxHealth;
        healthSlider.maxValue = rsoBoss.Get().Health.GetMaxHealth;

        healthSlider.value = newValue;
        this.Delay(() => innerHealthSlider.DOValue(newValue, animDuration).SetEase(Ease.OutCubic), delayBeforeAnim);
    }

    private void HandleDayCycleChange(DayCycleState newCycle)
    {
        if (newCycle == DayCycleState.Night)
        {
            healthSlider.gameObject.SetActive(true);
        }
        else
        {
            healthSlider.gameObject.SetActive(false);
        }
    }
}