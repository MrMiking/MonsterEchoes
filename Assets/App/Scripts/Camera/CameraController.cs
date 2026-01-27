using DG.Tweening;
using MVsToolkit.Dev;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum CameraMode { Follow, Fixed }

    [SerializeField] private RSO_DayCycle dayCycle;

    [Header("Settings")]
    [SerializeField] private CameraMode currentMode = CameraMode.Follow;
    [SerializeField] private Transform player;

    [Header("Follow Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -10);
    [SerializeField] private float smoothTime = 0.125f;
    Vector3 velocity;

    [Header("Fixed Settings")]
    [SerializeField] private Transform fixedPoint;

    public static CameraController Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void LateUpdate()
    {
        if (currentMode == CameraMode.Follow && player != null)
        {
            ApplyFollowMode();
        }
    }

    private void OnEnable()
    {
        dayCycle.OnChanged += UpdateCameraStateBasedOnCurrentCycle;
    }

    private void OnDisable()
    {
        dayCycle.OnChanged -= UpdateCameraStateBasedOnCurrentCycle;
    }

    private void UpdateCameraStateBasedOnCurrentCycle(DayCycleState state)
    {
        if (state == DayCycleState.Night)
        {
            SwitchMode(CameraMode.Fixed);
        }
        else
        {
            SwitchMode(CameraMode.Follow);
        }
    }

    private void ApplyFollowMode()
    {
        transform.position = Vector3.SmoothDamp(transform.position, player.position + offset, ref velocity, smoothTime);
    }

    public void SwitchMode(CameraMode newMode, float transitionDuration = 1f)
    {
        currentMode = newMode;

        if (currentMode == CameraMode.Fixed && fixedPoint != null)
        {
            transform.DOMove(fixedPoint.position, transitionDuration).SetEase(Ease.InOutSine);
            transform.DORotateQuaternion(fixedPoint.rotation, transitionDuration).SetEase(Ease.InOutSine);
        }
    }

    public void Shake(float intensity, float time)
    {
        transform.DOPunchRotation(Vector3.forward * intensity, time, 20, 1).OnComplete(() =>
        {
            transform.rotation = Quaternion.identity;
        });
    }
}