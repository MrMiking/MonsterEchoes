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
        public bool completed;
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

    public void MarkAsCompleted()
    {
        foreach (var d in dialogues)
        {
            if (!d.completed && dayCount.Get() >= d.dayCondition)
            {
                d.completed = true;
                break;
            }
        }
    }
}
