using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RSO_Boss boss;

    [Header("Health")]
    [SerializeField] private float animDuration = 0.2f;
    [SerializeField] private RSO_BossHealth rsoBossHealth;
    [SerializeField] private Slider bossHealthBar;

    private void OnEnable()
    {
        rsoBossHealth.OnChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        rsoBossHealth.OnChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float newValue)
    {
        Debug.Log("Health: " + newValue);
        bossHealthBar.maxValue = boss.Get().Health.GetMaxHealth;

        bossHealthBar.DOValue(newValue, animDuration).SetEase(Ease.OutCubic);
    }
}