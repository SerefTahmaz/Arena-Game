using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using UnityEngine;

public class GameHealthBarManager : cSingleton<GameHealthBarManager>
{
    [SerializeField] private cHealthBar m_BossUIHealthBar;
    [SerializeField] private cHealthBar m_PlayerUIHealthBar;
    [SerializeField] private bool m_IsBossUIBeingUsed;
    [SerializeField] private bool m_IsPlayerUIBeingUsed;
    
    private void Start()
    {
        m_BossUIHealthBar.m_OnVisibleUpdate += b =>
        {
            if (b == false)
            {
                m_IsBossUIBeingUsed = false;
            }
        };
        
        m_PlayerUIHealthBar.m_OnVisibleUpdate += b =>
        {
            if (b == false)
            {
                m_IsPlayerUIBeingUsed = false;
            }
        };
    }

    public cHealthBar GiveMeBossUIHealthBar()
    {
        if (m_IsBossUIBeingUsed)
        {
            return null;
        }
        else
        {
            m_IsBossUIBeingUsed = true;
            return m_BossUIHealthBar;
        }
    }
    
    public cHealthBar GiveMePlayerUIHealthBar()
    {
        if (m_IsPlayerUIBeingUsed)
        {
            return null;
        }
        else
        {
            m_IsPlayerUIBeingUsed = true;
            return m_PlayerUIHealthBar;
        }
    }

    public void ResetStates()
    {
        m_IsBossUIBeingUsed = false;
        m_IsPlayerUIBeingUsed = false;
    }
}
