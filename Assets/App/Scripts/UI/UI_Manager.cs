using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Manager : MonoBehaviour
{
    [Header("SETTINGS")]
    [SerializeField] private string m_OpenPanelName = "Menu";
    [SerializeField] private string m_ClosePanelName = "Game";

    [Header("REFERENCES")]
    [SerializeField] private InputActionReference m_OpenPauseMenuAction;

    [Header("INPUTS")]
    [SerializeField] private RSE_OpenScene m_OpenScene;
    [SerializeField] private RSE_QuitGame m_QuitGame;

    [Header("OUTPUTS")]
    [SerializeField] private RSE_OpenPanel m_OpenPanel;

    private bool m_IsOpen = false;

    private void OnEnable()
    {
        m_OpenPauseMenuAction.action.performed += PauseMenuButton;
        m_OpenPauseMenuAction.action.Enable();
        m_QuitGame.Action += Quit;
    }

    private void OnDisable()
    {
        m_OpenPauseMenuAction.action.performed -= PauseMenuButton;
        m_OpenPauseMenuAction.action.Disable();
        m_QuitGame.Action -= Quit;
    }

    private void PauseMenuButton(InputAction.CallbackContext ctx)
    {
        if (!m_IsOpen)
            OpenPauseMenu();
        else
            ClosePauseMenu();
    }

    private void OpenPauseMenu()
    {
        m_IsOpen = true;
        Cursor.visible = true;
        Time.timeScale = 0f;
        m_OpenPanel.Call(m_OpenPanelName);
    }

    private void ClosePauseMenu()
    {
        m_IsOpen = false;
        Cursor.visible = false;
        Time.timeScale = 1f;
        m_OpenPanel.Call(m_ClosePanelName);
    }

    private void Quit()
    {
        // Save Game Here
        Application.Quit();
    }
}