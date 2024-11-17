using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.UI;
using ArenaGame.Utils;
using STNest.Utils;
using UnityEngine;

public class cUIManager : cSingleton<cUIManager>
{
    [SerializeField] private CanvasGroup m_CanvasGroup;
    [SerializeField] private List<LockableItem> m_Views;
    [SerializeField] private cMenuNode m_MainMenuNode;
    [SerializeField] private cMenuNode m_StartMenuNode;

    public cMenuNode MainMenuNode => m_MainMenuNode;
    public cMenuNode StartMenuNode => m_StartMenuNode;

    private Dictionary<Page, LockableItem> m_LockablePages = new Dictionary<Page, LockableItem>();

    private void Awake()
    {
        foreach (var VARIABLE in m_Views)
        {
            m_LockablePages.Add(VARIABLE.Page, VARIABLE);
        }
    }

    public void ShowPage(Page page, object token,bool instant = false)
    {
        var lockablePage = m_LockablePages[page];
        lockablePage.EnablePage(token);

        if (!lockablePage.IsUnlocked)
        {
            Debug.Log($"Enabled {page}");
            lockablePage.View.Activate(instant);
        }
    }
    
    public void HidePage(Page page, object token,bool instant = false)
    {
        var lockablePage = m_LockablePages[page];
        lockablePage.DisablePage(token);

        if (lockablePage.IsUnlocked)
        {
            lockablePage.View.Deactivate(instant);
        }
    }

    public void SetInteractable(bool state)
    {
        m_CanvasGroup.interactable = state;
        m_CanvasGroup.blocksRaycasts = state;
    }
    
    [Serializable]
    public class LockableItem
    {
        [SerializeField] private Page m_Page;
        [SerializeField] private cMenuNode m_View;
        
        [SerializeField] private ObjectLock m_Lock = new ObjectLock();
        public bool IsUnlocked => m_Lock.LockState;
        public Page Page => m_Page;
        public cMenuNode View => m_View;

        public void EnablePage(object token)
        {
            m_Lock.ActivateLock(token);
        }

        public void DisablePage(object token)
        {
            m_Lock.DeactivateLock(token);
        }
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
