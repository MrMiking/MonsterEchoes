using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class UI_DialogueManager : MonoBehaviour
{
    [Header("RSE")]
    [SerializeField] private RSE_SendDialogue sendDialogue;
    [SerializeField] private RSE_CloseDialogue closeDialogue;
    
    [Header(("Text"))]
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    
    [Header("Button")]
    [SerializeField] private DialogueButton buttonPrefab;
    [SerializeField] private GameObject buttonParent;
    
    [Header("PlayerInput")]
    [SerializeField] private InputActionAsset inputAsset;
    
    [Header("Other")]
    [SerializeField] private GameObject dialoguePanel;

    private SSO_Dialogue currentDialogueData;
    private DialogueLine currentDialogueLine;
    private bool isActive = false;
    private int currentIndex = 0;
    
    private InputActionMap playerInputMap;
    private InputActionMap UIInputMap;

    private void Awake()
    {
        playerInputMap = inputAsset.FindActionMap("Player");
        UIInputMap = inputAsset.FindActionMap("UI");
    }

    private void OnEnable()
    {
        sendDialogue.Action += SetupDialogue;
    }

    private void OnDisable()
    {
        sendDialogue.Action -= SetupDialogue;
    }

    private void SetupDialogue(SSO_Dialogue data)
    {
        currentDialogueData = data;
        currentIndex = 0;

        // Activer le panel s'il est pas déja ouvert
        if (!isActive) OpenPanel();

        DisplayDialogueText();
    }

    private void DisplayDialogueText()
    {
        currentDialogueLine = currentDialogueData.lines[currentIndex];

        // Set Speaker Name
        if (currentDialogueLine.speakerName != speakerNameText.text && currentDialogueLine.speakerName != "")
        {
            speakerNameText.text = currentDialogueLine.speakerName;
        }
        else
        {
            speakerNameText.text = currentDialogueData.speakerName;
        }
        
        // Set Speaker Dialogues
        dialogueText.text = currentDialogueLine.speakerText;
        
        foreach (Transform child in buttonParent.transform)
        {
            Destroy(child.gameObject);
        }
        
        // Set Choices
        if (currentDialogueLine.choices.Length == 0)
        {
            if (currentIndex + 1 < currentDialogueData.lines.Length)
            {
                Debug.Log("Next Line");
            
                currentIndex++;
            
                DialogueButton button = Instantiate(buttonPrefab, buttonParent.transform);
            
                button.SetData("Continue", () => DisplayDialogueText());
            }
            else
            {
                DialogueButton button = Instantiate(buttonPrefab, buttonParent.transform);
            
                button.SetData("Close", () =>
                {
                    ClosePanel();
                    DayCycleManager.instance.HandleNextCycle();
                });
            }
        }
        else
        {
            foreach (DialogueChoice choice in currentDialogueLine.choices)
            {
                DialogueButton button = Instantiate(buttonPrefab, buttonParent.transform);

                if (choice.nextDialogue != null)
                {
                    choice.onChoiceSelected.AddListener(() => sendDialogue.Call(choice.nextDialogue));
                }
                else
                {
                    Debug.Log("No Next Dialogue, Add Choices");
                    choice.onChoiceSelected.AddListener(() => 
                    { 
                        ClosePanel();
                        DayCycleManager.instance.HandleNextCycle();
                    } );
                }
                
                button.SetData(choice.choiceText, () => choice.onChoiceSelected.Invoke());
            }
        }
    }

    private void OpenPanel()
    {
        playerInputMap.Disable();
        UIInputMap.Enable();
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        isActive = true;
        dialoguePanel.SetActive(isActive);
    }

    private void ClosePanel()
    {
        playerInputMap.Enable();
        UIInputMap.Disable();
        
        foreach (var action in playerInputMap.actions)
        {
            action.Disable();
            action.Enable();
        }
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        isActive = false;
        dialoguePanel.SetActive(isActive);
        
        closeDialogue.Call();
    }
}