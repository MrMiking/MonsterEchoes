using TMPro;
using UnityEngine;
using System.Collections;

public class UI_DialoguePanel : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float typingSpeed = 0.02f;
    [SerializeField] private float postTypingDelay = 0.5f;

    [Header("References")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI nameTMP;
    [SerializeField] private TextMeshProUGUI textTMP;
    [SerializeField] private Transform choiceContainer;
    [SerializeField] private DialogueButton buttonPrefab;

    private Coroutine typingCoroutine;
    private bool isTyping = false;

    private void Awake() => HidePanel();

    public void ShowPanel() => panel.SetActive(true);
    public void HidePanel() => panel.SetActive(false);

    public void SetupLine(string name, string text)
    {
        nameTMP.text = name;

        foreach (Transform child in choiceContainer) Destroy(child.gameObject);

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(text));
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        textTMP.text = "";

        foreach (char letter in text.ToCharArray())
        {
            textTMP.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(postTypingDelay);

        isTyping = false;
    }

    public void CreateChoice(string label, System.Action onClickAction)
    {
        StartCoroutine(ShowButtonWhenFinished(label, onClickAction));
    }

    private IEnumerator ShowButtonWhenFinished(string label, System.Action onClickAction)
    {
        while (isTyping)
        {
            yield return null;
        }

        DialogueButton button = Instantiate(buttonPrefab, choiceContainer.transform);
        button.SetData(label, onClickAction);
    }
}