using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using UnityEngine;

public class cLobbyCreationManager : cSingleton<cLobbyCreationManager>
{
    public struct LobbyCreationSettingWrapper
    {
        public string m_LobbyName;
        public bool m_IsPrivate;
        public int m_PlayerCount;
        public eGameMode m_GameMode;
    }

    public void OnCreate(LobbyCreationSettingWrapper setting, Action onCreated = null)
    {
        void Created()
        {
            onCreated?.Invoke();
        }

        Debug.Log("Main Change");
        cLobbyManager.Instance.CreateLobby(setting.m_LobbyName
            , setting.m_PlayerCount, 
            setting.m_IsPrivate, 
            setting.m_GameMode, 
            Created);
    }
}
