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
    [SerializeField] private cButton m_StartButton;
    
    // Start is called before the first frame update
    void Start()
    {
        m_View.OnActivateEvent.AddListener(HandleOnActivate);
        m_StartButton.OnClickEvent.AddListener(HandleStartButtonClicked);
    }

    private void HandleStartButtonClicked()
    {
        InventoryPreviewManager.Instance.SetState(false);
    }

    private void HandleOnActivate()
    {
        InventoryPreviewManager.Instance.SetState(true);
        CheckDisqualifiedState();
    }

    private void CheckDisqualifiedState()
    {
        SaveGameHandler.Load();
        if (SaveGameHandler.SaveData.m_IsPlayerDisqualified)
        {
            
            
            ExperienceManager.LoseExperience(35);
            CurrencyManager.SpendCurrency(25);
            GlobalFactory.DisqualifyPopUpFactory.Create();
            SaveGameHandler.SaveData.m_IsPlayerDisqualified = false;
            SaveGameHandler.Save();
        }
    }
}
