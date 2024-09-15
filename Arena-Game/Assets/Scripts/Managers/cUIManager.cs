using System.Collections;
using System.Collections.Generic;
using ArenaGame.UI;
using ArenaGame.Utils;
using UnityEngine;

public class cUIManager : cSingleton<cUIManager>
{
    [SerializeField] private CanvasGroup m_CanvasGroup;
    [SerializeField] private List<cView> m_Views;
    
    public void ShowPage(Page page)
    {
        m_Views[(int)page].Activate();
    }
    
    public void HidePage(Page page)
    {
        m_Views[(int)page].Deactivate();
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
    Gameplay
}
