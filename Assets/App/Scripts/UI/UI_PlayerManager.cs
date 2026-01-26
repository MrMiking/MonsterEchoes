using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RSO_Player rsoPlayer;
    
    [Header("Health")]
    [SerializeField] private float animDuration = 0.2f;
    [SerializeField] private RSO_Health rsoPlayerHealth;
    [SerializeField] private Slider healthSlider;

    private void OnEnable()
    {
        rsoPlayerHealth.OnChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        rsoPlayerHealth.OnChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float newValue)
    {
        Debug.Log("Health: " + newValue);
        healthSlider.maxValue = rsoPlayer.Get().Health.GetMaxHealth;
        
        healthSlider.DOValue(newValue, animDuration) .SetEase(Ease.OutCubic);
    }
}
