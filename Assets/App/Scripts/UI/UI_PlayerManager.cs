using DG.Tweening;
using MVsToolkit.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RSO_Player rsoPlayer;

    [Header("Health")]
    [SerializeField] private float delayBeforeAnim = 0.5f;
    [SerializeField] private float animDuration = 0.5f;
    [SerializeField] private RSO_Health rsoPlayerHealth;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider innerHealthSlider;

    [Header("Day")]
    [SerializeField] private TextMeshProUGUI dayCountText;
    [SerializeField] private TextMeshProUGUI dayCycleText;
    [SerializeField] private RSO_DayCycle dayCycle;
    [SerializeField] private RSO_DayCount dayCount;

    private void OnEnable()
    {
        rsoPlayerHealth.OnChanged += UpdateHealthBar;
        dayCount.OnChanged += UpdateDayCount;
        dayCycle.OnChanged += UpdateDayCycle;
    }

    private void OnDisable()
    {
        rsoPlayerHealth.OnChanged -= UpdateHealthBar;
        dayCount.OnChanged -= UpdateDayCount;
        dayCycle.OnChanged -= UpdateDayCycle;
    }

    private void UpdateHealthBar(float newValue)
    {
        innerHealthSlider.maxValue = rsoPlayer.Get().Health.GetMaxHealth;
        healthSlider.maxValue = rsoPlayer.Get().Health.GetMaxHealth;

        healthSlider.value = newValue;
        this.Delay(() => innerHealthSlider.DOValue(newValue, animDuration).SetEase(Ease.OutCubic), delayBeforeAnim);
        
    }

    private void UpdateDayCount(int newDay)
    {
        dayCountText.text = "Day " + newDay;
    }

    private void UpdateDayCycle(DayCycleState newState)
    {
        dayCycleText.text = newState.ToString();
    }
}
