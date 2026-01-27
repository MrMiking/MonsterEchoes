using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private InputActionReference interactInput;
    private IInteractable currentInteractable;

    private void OnEnable() => interactInput.action.performed += OnInteractPerformed;
    private void OnDisable() => interactInput.action.performed -= OnInteractPerformed;

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (currentInteractable != null && currentInteractable.CanInteract)
        {

            currentInteractable.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            currentInteractable = interactable;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            if (currentInteractable == interactable) currentInteractable = null;
        }
    }
}
