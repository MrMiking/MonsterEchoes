using UnityEngine;

public class NPCDialogue : MonoBehaviour, IDialogueProvider
{
    [SerializeField] private RSO_DayCount dayCount;
    [SerializeField] private SSO_Dialogue defaultDialogue;
    [SerializeField] private DialogueEntry[] dialogues;

    [System.Serializable]
    private class DialogueEntry
    {
        public SSO_Dialogue data;
        public int dayCondition;
        public SSO_Dialogue dialogueCondition;
        public bool completed;
        public bool available;
    }

    public void SetupAllDialogues()
    {
        foreach (DialogueEntry d in dialogues)
        {
            if (d.dialogueCondition != null)
            {
                d.dialogueCondition.onCompleted.AddListener(() => d.available = true);
            }
            else if (d.data != null)
            {
                d.data.onCompleted.AddListener(() => d.completed = true);
            }
        }
    }

    public SSO_Dialogue GetDialogue()
    {
        foreach (var d in dialogues)
        {
            if (!d.completed && dayCount.Get() >= d.dayCondition)
            {
                return d.data;
            }
        }
        return defaultDialogue;
    }
}
