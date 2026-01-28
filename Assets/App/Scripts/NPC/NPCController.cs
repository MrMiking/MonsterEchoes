using UnityEngine;

public class NPCController : MonoBehaviour, IInteractable
{
    public enum NPCInteractionState { Idle, Available, Interacting }

    [Header("References")]
    [SerializeField] private NPCVisual npcVisual;
    [SerializeField] private NPCDialogue npcDialogue;

    [Header("Events")]
    [SerializeField] private RSE_SendDialogue sendDialogue;
    [SerializeField] private RSE_CloseDialogue closeDialogue;

    private bool playerInRange;
    private NPCInteractionState currentState = NPCInteractionState.Idle;

    public bool CanInteract => currentState == NPCInteractionState.Available;

    private void Start() => npcVisual.ShowInteract(false);

    private void OnEnable()=> closeDialogue.Action += OnDialogueClosed;
    private void OnDisable()=> closeDialogue.Action -= OnDialogueClosed;

    public void Interact()
    {
        if (!CanInteract) return;

        currentState = NPCInteractionState.Interacting; 
        UIContextManager.Instance.PushContext(UIContext.Dialogue);

        RefreshInteractionState();

        SSO_Dialogue dialogue = npcDialogue.GetDialogue();
        if (dialogue != null)
        {
            npcDialogue.MarkAsCompleted();
            sendDialogue.Call(dialogue);
        }
    }

    private void OnDialogueClosed()
    {
        UIContextManager.Instance.PopContext(UIContext.Dialogue);
        currentState = playerInRange ? NPCInteractionState.Available : NPCInteractionState.Idle;
        RefreshInteractionState();
    }

    #region Trigger Detection
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;

        if (currentState == NPCInteractionState.Idle){
            currentState = NPCInteractionState.Available;
        }

        RefreshInteractionState();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        currentState = NPCInteractionState.Idle;

        RefreshInteractionState();
    }
    #endregion

    private void RefreshInteractionState()
    {
        npcVisual.ShowInteract(CanInteract);
    }
}