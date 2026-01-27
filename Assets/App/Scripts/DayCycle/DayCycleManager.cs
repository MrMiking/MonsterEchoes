using DG.Tweening;
using MVsToolkit.Dev;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayCycleManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private DayCycleState startingCycle;
    [SerializeField] private int startingDay = 1;
    [SerializeField] private bool bloodMoon = false;
    [SerializeField] private float duration = 2f;
    [Space(10)]
    [SerializeField] private Color dayAmbientColor;
    [SerializeField] private Color eveningAmbientColor;
    [SerializeField] private Color nightAmbientColor;
    [SerializeField] private Color bloodMoonAmbientColor;

    [Header("References")]
    [SerializeField] private Light2D ambientLight;

    [Header("Output")]
    [SerializeField] private RSO_DayCount dayCount;
    [SerializeField] private RSO_DayCycle currentCycle;

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

        switch(startingCycle)
        {
            case DayCycleState.Day:
                StartCoroutine(HandleDay());
                break;
            case DayCycleState.Evening:
                StartCoroutine(HandleEvening());
                break;
            case DayCycleState.Night:
                StartCoroutine(HandleNight());
                break;
        }

        dayCount.Set(startingDay);
    }

    [Button]
    public void HandleNextCycle()
    {
        switch (currentCycle.Get())
        {
            case DayCycleState.Day:
                StartCoroutine(HandleEvening());
                break;
            case DayCycleState.Evening:
                StartCoroutine(HandleNight());
                break;
            case DayCycleState.Night:
                StartCoroutine(HandleDay());
                break;
        }
    }

    private IEnumerator HandleDay()
    {
        if (bloodMoon) SetNewColor(bloodMoonAmbientColor);
        else SetNewColor(dayAmbientColor);

        yield return new WaitForSeconds(duration);

        dayCount.Set(dayCount.Get() + 1);
        currentCycle.Set(DayCycleState.Day);
    }

    private IEnumerator HandleEvening()
    {
        SetNewColor(eveningAmbientColor);

        yield return new WaitForSeconds(duration);

        currentCycle.Set(DayCycleState.Evening);
    }

    private IEnumerator HandleNight()
    {
        SetNewColor(nightAmbientColor);

        yield return new WaitForSeconds(duration);

        currentCycle.Set(DayCycleState.Night);
    }

    private void SetNewColor(Color targetColor)
    {
        DOTween.To(() => ambientLight.color,
                   x => ambientLight.color = x,
                   targetColor,
                   duration)
               .SetEase(Ease.InOutSine);
    }
}