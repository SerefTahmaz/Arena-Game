using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using Cysharp.Threading.Tasks;
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

    public async UniTask<RequestResult> OnCreate(LobbyCreationSettingWrapper setting)
    {
        var token = new object();
        MiniLoadingScreen.Instance.ShowPage(token);
        Debug.Log("Main Change");
        var result = await cLobbyManager.Instance.CreateLobby(setting.m_LobbyName
            , setting.m_PlayerCount, 
            setting.m_IsPrivate, 
            setting.m_GameMode);
        MiniLoadingScreen.Instance.HidePage(token);
        return result;
    }
}
