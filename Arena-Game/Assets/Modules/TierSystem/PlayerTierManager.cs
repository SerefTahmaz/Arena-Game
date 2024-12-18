using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Managers.SaveManager;
using ArenaGame.Utils;
using UnityEngine;

public class PlayerTierManager : cSingleton<PlayerTierManager>
{
    public int CurrentTier => UtilitySaveHandler.SaveData.m_CurrentTierIndex;

    private void Awake()
    {
        cGameManager.Instance.OnPlayerWin += HandleWin;
        cGameManager.Instance.OnPlayerLose += HandleLose;
    }

    private void OnDestroy()
    {
        if(cGameManager.Instance == null) return;
        cGameManager.Instance.OnPlayerWin -= HandleWin;
        cGameManager.Instance.OnPlayerLose -= HandleLose;
    }

    private void HandleLose()
    {
        var gameMode = cGameManager.Instance.CurrentGameMode;
        if(!(gameMode == eGameMode.PvP || gameMode == eGameMode.PvPSingle)) return;
        
        var tierLoseCount = UtilitySaveHandler.SaveData.m_TierLoses;
        tierLoseCount++;

        if (tierLoseCount >= 4)
        {
            UtilitySaveHandler.SaveData.m_TierLoses = 0;
            UtilitySaveHandler.SaveData.m_CurrentTierIndex = Mathf.Max(0, CurrentTier - 1);
        }
        else
        {
            UtilitySaveHandler.SaveData.m_TierLoses++;
        }
        UtilitySaveHandler.Save();
    }

    private void HandleWin()
    {
        var gameMode = cGameManager.Instance.CurrentGameMode;
        if(!(gameMode == eGameMode.PvP || gameMode == eGameMode.PvPSingle)) return;
        
        var tierWinCount = UtilitySaveHandler.SaveData.m_TierWins;
        tierWinCount++;

        if (tierWinCount >= 4)
        {
            UtilitySaveHandler.SaveData.m_TierWins = 0;
            UtilitySaveHandler.SaveData.m_CurrentTierIndex = Mathf.Min(3, CurrentTier + 1);
        }
        else
        {
            UtilitySaveHandler.SaveData.m_TierWins++;
        }

        UtilitySaveHandler.Save();
    }
}
