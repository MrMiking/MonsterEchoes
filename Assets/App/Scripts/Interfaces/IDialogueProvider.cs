using UnityEngine;

public interface IDialogueProvider
{
    SSO_Dialogue GetDialogue();
    void MarkAsCompleted();
}
