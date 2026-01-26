using UnityEngine;
using UnityEngine.InputSystem;

public class Dialogue : MonoBehaviour
{
    [Header("Temp")]
    [SerializeField] private int night = 0;
    
    [Header("References")]
    [SerializeField] private Dialogues[] dialogueData;
    [SerializeField] private SSO_Dialogue defaultDialogue;
    [SerializeField] private RSE_SendDialogue sendDialogue;
    [SerializeField] private RSE_CloseDialogue closeDialogue;
    [SerializeField] private GameObject inputVisual;
    
    [Header("Inputs")]
    [SerializeField] private InputActionReference interactInput;

    private bool canInteract;

    [System.Serializable]
    private class Dialogues
    {
        public SSO_Dialogue dialogueData;
        public int timeCondition;
        public bool completed;
    }

    private void Awake()
    {
        inputVisual.SetActive(false);
        
        interactInput.action.Enable();
    }

    private void OnEnable()
    {
        interactInput.action.performed += TryInteract;

        closeDialogue.Action += CloseDialogue;
    }

    private void OnDisable()
    {
        interactInput.action.performed -= TryInteract;
        
        closeDialogue.Action -= CloseDialogue;
    }

    private void TryInteract(InputAction.CallbackContext context)
    {
        if (!canInteract) return;
        
        canInteract = false;
        inputVisual.SetActive(false);

        foreach (Dialogues dialogue in dialogueData)
        {
            if (dialogue.dialogueData == null) break;
            
            if (!dialogue.completed && dialogue.timeCondition >= night)
            {
                dialogue.completed = true;
                sendDialogue.Call(dialogue.dialogueData);
                return;
            }
        }
        
        sendDialogue.Call(defaultDialogue);
    }

    private void CloseDialogue()
    {
        canInteract = true;
        inputVisual.SetActive(true);
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