using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI choiceText;
    [SerializeField] private Button choiceButton;

    public void SetData(string choiceText, Action choiceButtonEvent)
    {
        this.choiceText.text = choiceText;
        choiceButton.onClick.AddListener(choiceButtonEvent.Invoke);
    }
}
