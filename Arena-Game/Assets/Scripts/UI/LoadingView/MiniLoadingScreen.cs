using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.UI;
using ArenaGame.Utils;
using STNest.Utils;
using UnityEngine;

public class MiniLoadingScreen : cSingleton<MiniLoadingScreen>
{
    [SerializeField] private cUIManager.LockableItem m_LoadingView;
 
    public void ShowPage(object token,bool instant = false)
    {
        var lockablePage = m_LoadingView;
        lockablePage.EnablePage(token);

        if (lockablePage.IsActive)
        {
            if (lockablePage.View.IsActive)
            {
                lockablePage.View.Activate(true);
            }
            else
            {
                lockablePage.View.Activate(instant);
            }
        }
    }
    
    public void HidePage(object token,bool instant = false)
    {
        var lockablePage = m_LoadingView;
        lockablePage.DisablePage(token);

        if (!lockablePage.IsActive)
        {
            lockablePage.View.Deactivate(instant);
        }
    }
}