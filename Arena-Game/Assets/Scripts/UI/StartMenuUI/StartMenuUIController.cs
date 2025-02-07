using System.Collections;
using System.Collections.Generic;
using ArenaGame.Currency;
using ArenaGame.Experience;
using ArenaGame.Managers.SaveManager;
using ArenaGame.UI;
using UnityEngine;

public class StartMenuUIController : MonoBehaviour
{
    [SerializeField] private cView m_View;
    [SerializeField] private StartButtonController m_StartButton;
    [SerializeField] private cView m_ProfileView;
    [SerializeField] private cButton m_FreeroamButton;
    
    // Start is called before the first frame update
    void Start()
    {
        m_View.OnActivateEvent.AddListener(HandleOnActivate);
        m_StartButton.OnClickEvent.AddListener(HandleStartButtonClicked);
        m_FreeroamButton.OnClickEvent.AddListener(HandleStartButtonClicked);
    }

    private void HandleStartButtonClicked()
    {
        InventoryPreviewManager.Instance.SetState(false);
        m_ProfileView.Deactivate();
    }

    private void HandleOnActivate()
    {
        InventoryPreviewManager.Instance.SetState(true);
        CheckClosedGameInGameplay();
        CheckDisqualifiedState();
        m_ProfileView.Activate();
    }

    private void CheckClosedGameInGameplay()
    {
        UserSaveHandler.Load();
        if (UserSaveHandler.SaveData.m_IsPlayerClosedAppInGameplay)
        {
            UserSaveHandler.SaveData.m_IsPlayerClosedAppInGameplay = false;
            UserSaveHandler.SaveData.m_IsPlayerDisqualified = true;
            Debug.Log("Closed in gameplay Disqualified !!");
            UserSaveHandler.Save();
        }
    }

    private void CheckDisqualifiedState()
    {
        UserSaveHandler.Load(); 
        if (UserSaveHandler.SaveData.m_IsPlayerDisqualified)
        {
            GlobalFactory.DisqualifyPopUpFactory.Create();
            UserSaveHandler.SaveData.m_IsPlayerDisqualified = false;
            UserSaveHandler.Save();
        }
    }
}
