using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Gameplay.Character;
using Unity.Netcode;
using UnityEngine;

public class cPvPManager : MonoBehaviour,IGameModeHandler
{
    private int m_SpawnOffset;
    private bool m_isActive;

    private void OnMainMenuButton()
    {
        if (m_isActive)
        {
            m_isActive = false;
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            cGameManager.Instance.m_OnMainMenuButton -= OnMainMenuButton;
        }
    }

    public void StartGame()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            cGameManager.Instance.m_OnPlayerDied = delegate { };
            cGameManager.Instance.m_OnPlayerDied += CheckPvPSuccess;
            
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            cGameManager.Instance.m_OnMainMenuButton += OnMainMenuButton;

            m_isActive = true;
            
            LoopStart();
        }
    }

    private void LoopStart()
    {
        cUIManager.Instance.HidePage(Page.MainMenu);
        cUIManager.Instance.ShowPage(Page.Gameplay);
        cUIManager.Instance.ShowPage(Page.Loading);
        m_SpawnOffset = 0;
        cPlayerManager.Instance.DestroyPlayers();
        foreach (var VARIABLE in NetworkManager.Singleton.ConnectedClients)
        {
            OnClientConnected(VARIABLE.Key);
        }

        DOVirtual.DelayedCall(5, () =>
        {
            cUIManager.Instance.HidePage(Page.Loading);
        });
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
    }

    private void CheckPvPSuccess()
    {
        if (cPlayerManager.Instance.CheckExistLastStandingPlayer())
        {
            DOVirtual.DelayedCall(5, () =>
            {
                if (m_isActive)
                {
                    LoopStart();
                }
            });
        }
    }
}

public interface IGameModeHandler
{
    public void StartGame();
}