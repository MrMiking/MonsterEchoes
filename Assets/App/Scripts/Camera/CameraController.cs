using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum CameraMode { Follow, Fixed }

    [Header("Settings")]
    [SerializeField] private CameraMode currentMode = CameraMode.Follow;
    [SerializeField] private Transform player;

    [Header("Follow Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -10);
    [SerializeField] private float smoothSpeed = 0.125f;

    [Header("Fixed Settings")]
    [SerializeField] private Transform fixedPoint;

    private void LateUpdate()
    {
        if (currentMode == CameraMode.Follow && player != null)
        {
            ApplyFollowMode();
        }
    }

    private void ApplyFollowMode()
    {
        Vector3 desiredPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.LookAt(player);
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
}