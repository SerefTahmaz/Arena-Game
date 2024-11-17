using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Managers.SaveManager;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Character;
using Unity.Netcode;
using UnityEngine;

public class cPvPManager : MonoBehaviour,IGameModeHandler
{
    private int m_SpawnOffset;
    private bool m_IsActive;
    private int m_ConnectedClientCounts;

    public void StartGame()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            cGameManager.Instance.m_OnPlayerDied = delegate { };
            cGameManager.Instance.m_OnPlayerDied += CheckPvPSuccess;
            
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            cGameManager.Instance.m_OnMainMenuButton += OnMainMenuButton;

            NetworkManager.Singleton.OnServerStopped += HandleServerStopped;

            m_IsActive = true;
            
            LoopStart();
        }
    }

    private void HandleServerStopped(bool obj)
    {
        // Debug.Log("Server Stopped!!!!!!!");
    }

    private async UniTask LoopStart()
    {
        cUIManager.Instance.ShowPage(Page.Gameplay,this);
        cUIManager.Instance.ShowPage(Page.Loading,this);
        
        SaveGameHandler.Load();
        var currentMap = SaveGameHandler.SaveData.m_CurrentMap;
        await MapManager.instance.SetMap(currentMap);
        
        m_SpawnOffset = 0;
        cPlayerManager.Instance.DestroyPlayers();
        foreach (var VARIABLE in NetworkManager.Singleton.ConnectedClients)
        {
            OnClientConnected(VARIABLE.Key);
        }

        await UniTask.WaitUntil((() => m_ConnectedClientCounts >= cLobbyManager.Instance.JoinedLobby.Players.Count));
        MultiplayerLocalHelper.instance.NetworkHelper.m_IsGameStarted.Value = true;
        cUIManager.Instance.HidePage(Page.Loading,false);
    }
    
    private void OnClientConnected(ulong obj)
    {
        Vector3 pos;
        GameObject go;
        pos = (Vector3.right * Mathf.Cos(m_SpawnOffset*90) + Vector3.forward * Mathf.Sin(m_SpawnOffset*90))*5;
        Vector3 dir = Vector3.zero - pos;
        var lookRot = Quaternion.LookRotation(dir.normalized);
        go = cPlayerManager.Instance.SpawnPlayer(pos, lookRot, obj);
        go.GetComponent<HumanCharacter>().CharacterNetworkController.m_TeamId.Value = 10 + m_SpawnOffset;
        m_SpawnOffset++;
        m_ConnectedClientCounts++;
    }

    private void CheckPvPSuccess()
    {
        if (cPlayerManager.Instance.CheckExistLastStandingPlayer())
        {
            Debug.Log($"Game Ended");

            OnGameEnd();
            MultiplayerLocalHelper.instance.NetworkHelper.CheckGameEndClientRpc();
        }
    }
    
    private void OnMainMenuButton()
    {
        if (m_IsActive)
        {
            OnGameEnd();
        }
    }
    
    private void OnGameEnd()
    {
        m_IsActive = false;
        cGameManager.Instance.m_OnMainMenuButton -= OnMainMenuButton;
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        cUIManager.Instance.HidePage(Page.Gameplay,this);
        Debug.Log("GameEnded!!!!!!!!!!");
    }
}

public interface IGameModeHandler
{
    public void StartGame();
}