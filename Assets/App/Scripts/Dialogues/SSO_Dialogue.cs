using MVsToolkit.Dev;
using UnityEngine;

public enum DialogueType
{
    Monologue,
    Choice,
}

[CreateAssetMenu(fileName = "SSO Dialogue", menuName = "Dialogue/SSO Dialogue")]
public class SSO_Dialogue : ScriptableObject
{
    [Header("Speaker Info")]
    public string speakerName;
    public Sprite speakerVisual;

    [Header("Content")]
    public DialogueLine[] lines;
}

[System.Serializable]
public struct DialogueLine
{
    public DialogueType dialogueType;
    public string speakerName;
    [TextArea(3, 10)] public string speakerText;
    [ShowIf("DialogueType.Choice", "dialogueType")] public DialogueChoice[] choices;
}

[System.Serializable]
public struct DialogueChoice
{
    public string choiceText;
    public SSO_Dialogue nextDialogue;
    public bool closeDialogue;
    public bool advanceDay;
    public UnityEngine.Events.UnityEvent onChoiceSelected;
}