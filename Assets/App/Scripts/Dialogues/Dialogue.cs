using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private SSO_Dialogue dialogueData;
    [SerializeField] private RSE_SendDialogue sendDialogue;

    public void Interact()
    {
        sendDialogue.Call(dialogueData);
    }
}