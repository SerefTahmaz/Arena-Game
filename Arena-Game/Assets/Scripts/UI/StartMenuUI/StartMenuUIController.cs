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
        SaveGameHandler.Load();
        if (SaveGameHandler.SaveData.m_IsPlayerClosedAppInGameplay)
        {
            SaveGameHandler.SaveData.m_IsPlayerClosedAppInGameplay = false;
            SaveGameHandler.SaveData.m_IsPlayerDisqualified = true;
            Debug.Log("Closed in gameplay Disqualified !!");
            SaveGameHandler.Save();
        }
    }

    private void CheckDisqualifiedState()
    {
        SaveGameHandler.Load(); 
        if (SaveGameHandler.SaveData.m_IsPlayerDisqualified)
        {
            GlobalFactory.DisqualifyPopUpFactory.Create();
            SaveGameHandler.SaveData.m_IsPlayerDisqualified = false;
            SaveGameHandler.Save();
        }
    }
}
