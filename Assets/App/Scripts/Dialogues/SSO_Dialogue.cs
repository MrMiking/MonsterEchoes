using MVsToolkit.Dev;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SSO Dialogue", menuName = "Dialogue/SSO Dialogue")]
public class SSO_Dialogue : ScriptableObject
{
    [Header("Speaker Info")]
    public string speakerName;
    public Sprite speakerVisual;

    [Header("Content")]
    public DialogueLine[] lines;

    [HideInInspector] public UnityEvent onCompleted;
}

[System.Serializable]
public struct DialogueLine
{
    public string speakerName;
    [TextArea(3, 10)] public string speakerText; 
    public DialogueChoice[] choices;
}

[System.Serializable]
public struct DialogueChoice
{
    public string choiceText;
    public SSO_Dialogue nextDialogue;
    public bool failed;
    public bool closeDialogue;
    public bool advanceDay;
    public UnityEngine.Events.UnityEvent onChoiceSelected;
}