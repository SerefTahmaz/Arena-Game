using System.Collections;
using System.Collections.Generic;
using ArenaGame.UI;
using ArenaGame.Utils;
using UnityEngine;

public class cUIManager : cSingleton<cUIManager>
{
    [SerializeField] private CanvasGroup m_CanvasGroup;
    [SerializeField] private List<cView> m_Views;
    [SerializeField] private cMenuNode m_MainMenuNode;

    public cMenuNode MainMenuNode => m_MainMenuNode;


    public void ShowPage(Page page, bool instant = false)
    {
        m_Views[(int)page].Activate(instant);
    }
    
    public void HidePage(Page page, bool instant = false)
    {
        m_Views[(int)page].Deactivate(instant);
    }

    public void SetInteractable(bool state)
    {
        m_CanvasGroup.interactable = state;
        m_CanvasGroup.blocksRaycasts = state;
    }
}

public enum Page
{
    MainMenu,
    Loading,
    Gameplay,
    Win,
    Lose,
    StartMenu
}
