using UnityEngine;
using UnityEngine.InputSystem;

public class Dialogue : MonoBehaviour
{ 
    [Header("References")]
    [SerializeField] private RSO_DayCycle currentCycle;
    [SerializeField] private RSO_DayCount dayCount;
    [SerializeField] private Dialogues[] dialogueData;
    [SerializeField] private SSO_Dialogue defaultDialogue;
    [SerializeField] private RSE_SendDialogue sendDialogue;
    [SerializeField] private RSE_CloseDialogue closeDialogue;
    [SerializeField] private GameObject interactVisual;
    [SerializeField] private GameObject npcVisual;
    
    [Header("Inputs")]
    [SerializeField] private InputActionReference interactInput;

    private bool canInteract;
    private bool isEnable;

    [System.Serializable]
    private class Dialogues
    {
        public SSO_Dialogue dialogueData;
        public int timeCondition;
        public bool completed;
    }

    private void Awake()
    {
        interactVisual.SetActive(false);
        
        interactInput.action.Enable();
    }

    private void OnEnable()
    {
        interactInput.action.performed += TryInteract;

        closeDialogue.Action += CloseDialogue;

        currentCycle.OnChanged += UpdateNPC;
    }

    private void OnDisable()
    {
        interactInput.action.performed -= TryInteract;
        
        closeDialogue.Action -= CloseDialogue;

        currentCycle.OnChanged -= UpdateNPC;
    }

    private void UpdateNPC(DayCycleState state)
    {
        if(state == DayCycleState.Night)
        {
            isEnable = false;
            npcVisual.SetActive(false);
            interactVisual.SetActive(false);
        }
        else
        {
            isEnable = true;
            npcVisual.SetActive(true);
        }
    }

    private void TryInteract(InputAction.CallbackContext context)
    {
        if (!canInteract) return;
        if (!isEnable) return;
        
        canInteract = false;
        interactVisual.SetActive(false);

        foreach (Dialogues dialogue in dialogueData)
        {
            if (dialogue.dialogueData == null) break;
            
            if (!dialogue.completed && dialogue.timeCondition >= dayCount.Get())
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
        interactVisual.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!isEnable) return;
        if (other.CompareTag("Player")) canInteract = true;
        interactVisual.SetActive(canInteract);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!isEnable) return;
        if (other.CompareTag("Player")) canInteract = false;
        interactVisual.SetActive(canInteract);
    }
}