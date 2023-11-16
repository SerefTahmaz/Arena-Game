using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Netcode;
using UnityEngine;

public class cPvPManager : MonoBehaviour,IGameModeHandler
{
    private int m_SpawnOffset;
    private Action m_OnGameStart = delegate {  };
    private Action m_OnGameEnd = delegate {  };

    public Action OnGameEnd
    {
        get => m_OnGameEnd;
        set => m_OnGameEnd = value;
    }

    public Action OnGameStart
    {
        get => m_OnGameStart;
        set => m_OnGameStart = value;
    }

    public void StartGame()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            cGameManager.Instance.m_OnPlayerDied = delegate { };
            cGameManager.Instance.m_OnPlayerDied += CheckPvPSuccess;
            
            m_SpawnOffset = 0;
            cPlayerManager.Instance.DestroyPlayers();
            foreach (var VARIABLE in NetworkManager.Singleton.ConnectedClients)
            {
                OnClientConnected(VARIABLE.Key);
            }
            
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            
            OnGameStart.Invoke();
            
            DOVirtual.DelayedCall(5, (() =>
            {
                StartNextGame();
            }));
        }
    }

    private void OnClientConnected(ulong obj)
    {
        Vector3 pos;
        GameObject go;
        pos = (Vector3.right * Mathf.Cos(m_SpawnOffset*90) + Vector3.forward * Mathf.Sin(m_SpawnOffset*90))*5;
        Vector3 dir = Vector3.zero - pos;
        var lookRot = Quaternion.LookRotation(dir.normalized);
        go = cPlayerManager.Instance.SpawnPlayer(pos, lookRot, obj);
        go.GetComponent<cPlayerCharacter>().CharacterNetworkController.m_TeamId.Value = 10 + m_SpawnOffset;
        m_SpawnOffset++;
    }

    public void StartNextGame()
    {
       
    }

    private void CheckPvPSuccess()
    {
        if (cPlayerManager.Instance.CheckExistLastStandingPlayer())
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            DOVirtual.DelayedCall(5, () => StartGame());
        }
    }
}

public interface IGameModeHandler
{
    public Action OnGameEnd { get; set; }
    public Action OnGameStart { get; set; }
    public void StartGame();
}