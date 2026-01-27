using UnityEngine;

public class NPCVisual : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject interactVisual;

    public void ShowInteract(bool visible)
    {
        if (interactVisual.activeSelf == visible) return;
        interactVisual.SetActive(visible);
    }
}
