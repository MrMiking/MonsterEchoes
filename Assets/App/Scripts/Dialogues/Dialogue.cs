using System;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dialogue : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SSO_Dialogue dialogueData;
    [SerializeField] private RSE_SendDialogue sendDialogue;
    [SerializeField] private GameObject inputVisual;
    
    [Header("Inputs")]
    [SerializeField] private InputActionReference interactInput;

    private bool canInteract;

    private void Awake()
    {
        inputVisual.SetActive(false);
        
        interactInput.action.Enable();
    }

    private void OnEnable()
    {
        interactInput.action.performed += TryInteract;
    }

    private void OnDisable()
    {
        interactInput.action.performed -= TryInteract;
    }

    private void TryInteract(InputAction.CallbackContext context)
    {
        if (!canInteract) return;
        
        inputVisual.SetActive(false);
        
        sendDialogue.Call(dialogueData);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) canInteract = true;
        inputVisual.SetActive(canInteract);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) canInteract = false;
        inputVisual.SetActive(canInteract);
    }
}