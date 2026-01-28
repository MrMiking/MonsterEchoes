using DG.Tweening;
using MVsToolkit.Dev;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayCycleManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private DayCycleState startingCycle;
    [SerializeField] private int startingDay = 0;
    [SerializeField] private bool bloodMoon = false;
    [SerializeField] private float duration = 2f;
    [Space(10)]
    [SerializeField] private Color dayAmbientColor;
    [SerializeField] private Color eveningAmbientColor;
    [SerializeField] private Color nightAmbientColor;
    [SerializeField] private Color bloodMoonAmbientColor;
    [Space(10)]
    [SerializeField] private Color daySkyColor;
    [SerializeField] private Color eveningSkyColor;
    [SerializeField] private Color nightSkyColor;
    [SerializeField] private Color bloodMoonSkyColor;
    [Space(10)]
    [SerializeField] private Color baseMoonColor;
    [SerializeField] private Color bloodMoonColor;

    [Header("References")]
    [SerializeField] private Light2D ambientLight;
    [SerializeField] private Transform skyTransform;
    [SerializeField] private SpriteRenderer skyBackground;
    [SerializeField] private SpriteRenderer moon;

    [Header("Output")]
    [SerializeField] private RSO_DayCount dayCount;
    [SerializeField] private RSO_DayCycle currentCycle;

    public static DayCycleManager Instance;

    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
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
        SetNewColor(dayAmbientColor, daySkyColor, baseMoonColor);

        skyTransform.rotation = Quaternion.Euler(0, 0, 75);
        skyTransform.DORotate(new Vector3(0, 0, 200), duration).SetEase(Ease.InOutSine);

        yield return new WaitForSeconds(duration);

        dayCount.Value++;
        currentCycle.Value = DayCycleState.Day;
    }

    private IEnumerator HandleEvening()
    {
        SetNewColor(eveningAmbientColor, eveningSkyColor, baseMoonColor);

        skyTransform.rotation = Quaternion.Euler(0, 0, 0);

        yield return new WaitForSeconds(duration);

        currentCycle.Value = DayCycleState.Evening;
    }

    private IEnumerator HandleNight()
    {
        if (bloodMoon) SetNewColor(bloodMoonAmbientColor, bloodMoonSkyColor, bloodMoonColor);
        else SetNewColor(nightAmbientColor, nightSkyColor, baseMoonColor);

        skyTransform.rotation = Quaternion.Euler(0, 0, 0);
        skyTransform.DORotate(new Vector3(0, 0, 75), duration).SetEase(Ease.InOutSine);

        yield return new WaitForSeconds(duration);

        currentCycle.Value = DayCycleState.Night;
    }

    private void SetNewColor(Color targetAmbientColor, Color targetSkyColor, Color moonColor)
    {
        DOTween.To(() => ambientLight.color,
                   x => ambientLight.color = x,
                   targetAmbientColor,
                   duration)
               .SetEase(Ease.InOutSine);

        DOTween.To(() => skyBackground.color,
                  x => skyBackground.color = x,
                  targetSkyColor,
                  duration)
              .SetEase(Ease.InOutSine);
        
        DOTween.To(() => moon.color,
                x => moon.color = x,
                moonColor,
                duration)
            .SetEase(Ease.InOutSine);
    }
}