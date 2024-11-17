using System.Collections;
using System.Collections.Generic;
using ArenaGame.UI;
using UnityEngine;
using UnityEngine.Events;

public class PauseMenuUIController : MonoBehaviour
{
    [SerializeField] private cButton m_MainMenuButton;
    [SerializeField] private cButton m_ResumeButton;
    [SerializeField] private UnityEvent m_OnMainMenuButtonClicked;
    [SerializeField] private cView m_PaueMenuView;
    
    // Start is called before the first frame update
    void Start()
    {
        m_MainMenuButton.OnClickEvent.AddListener(HandleMainMenuClicked);
        m_ResumeButton.OnClickEvent.AddListener(HandleResumeClicked);
        m_PaueMenuView.OnActivateEvent.AddListener(HandlePauseMenuActivate);
    }

    private void HandleResumeClicked()
    {
        m_PaueMenuView.Deactivate();
        cGameManager.Instance.SetInput(true);
    }

    private void HandlePauseMenuActivate()
    {
        cGameManager.Instance.SetInput(false);
    }

    private void HandleMainMenuClicked()
    {
        m_PaueMenuView.Deactivate();
        cGameManager.Instance.HandleFreeroamEnd();
    }
}
