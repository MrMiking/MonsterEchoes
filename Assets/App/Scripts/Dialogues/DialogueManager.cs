using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UI_DialoguePanel dialoguePanel;

    [Header("Input")]
    [SerializeField] private RSE_SendDialogue sendDialogue;

    [Header("Output")]
    [SerializeField] private RSE_CloseDialogue closeDialogue;

    private int lineIndex = 0;
    private SSO_Dialogue currentDialogue;

    private void OnEnable() => sendDialogue.Action += StartDialogue;
    private void OnDisable() => sendDialogue.Action -= StartDialogue;

    private void StartDialogue(SSO_Dialogue dialogue)
    {
        currentDialogue = dialogue;
        lineIndex = 0;
        dialoguePanel.ShowPanel();

        ShowCurrentLine();
    }

    private void ShowCurrentLine()
    {
        DialogueLine line = currentDialogue.lines[lineIndex];

        string nameToDisplay = string.IsNullOrEmpty(line.speakerName)
                           ? currentDialogue.speakerName
                           : line.speakerName;

        dialoguePanel.SetupLine(nameToDisplay, line.speakerText);

        if (lineIndex < currentDialogue.lines.Length - 1 && line.choices.Length == 0)
        {
            dialoguePanel.CreateChoice("Next", () =>
            {
                lineIndex++;
                ShowCurrentLine();
            });
        }
        else
        {
            if (line.choices.Length == 0)
            {
                dialoguePanel.CreateChoice("Close", CloseDialogue);
            }
            else
            {
                foreach (DialogueChoice choice in line.choices)
                {
                    dialoguePanel.CreateChoice(choice.choiceText, () => HandleChoice(choice));
                }
            }
        }
    }

    private void HandleChoice(DialogueChoice choice)
    {
        choice.onChoiceSelected?.Invoke();

      
        if (choice.advanceDay)
        {
            DayCycleManager.Instance.HandleNextCycle();
        }

        if (!choice.failed)
        {
            currentDialogue.onCompleted.Invoke();
        }

        if (choice.closeDialogue)
        {
            CloseDialogue();
        }
        else
        {
            if (choice.nextDialogue == null)
            {

                StartDialogue(choice.nextDialogue);
            }
            else
            {
                lineIndex++;
                ShowCurrentLine();
            }
        }
    }

    private void CloseDialogue()
    {
        dialoguePanel.HidePanel();
        closeDialogue.Call();
    }
}