using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UI_DialogueManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RSE_SendDialogue sendDialogue;
    [Space(5)]
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private DialogueButton buttonPrefab;
    [SerializeField] private GameObject buttonParent;

    private SSO_Dialogue currentDialogueData;
    private DialogueLine currentDialogueLine;
    private bool isActive = false;
    private int currentIndex = 0;

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
        if (!isActive)
        {
            isActive = true;
            dialoguePanel.SetActive(isActive);
        }

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
        
        // Set Choices
        if (currentDialogueLine.choices.Length == 0 && currentIndex < currentDialogueData.lines.Length)
        {
            Debug.Log("Next Line");
            
            currentIndex++;
            
            DialogueButton button = Instantiate(buttonPrefab, buttonParent.transform);
            
            button.SetData("Continue", () => DisplayDialogueText());
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
                    Debug.Log("No Next Dialogue, close Panel");
                    choice.onChoiceSelected.AddListener(() => ClosePanel());
                }
                
                button.SetData(choice.choiceText, () => choice.onChoiceSelected.Invoke());
            }
        }
    }

    private void ClosePanel()
    {
        isActive = false;
        dialoguePanel.SetActive(isActive);
    }
}