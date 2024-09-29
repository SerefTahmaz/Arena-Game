using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.UI;
using ArenaGame.Utils;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Events;

public class cLobbyUI : cSingleton<cLobbyUI>
{
    [SerializeField] private cLobbyUnitUI m_LobbyUnit;
    [SerializeField] private cPlayerUnit m_PlayerUnit;
    [SerializeField] private Transform m_LayoutTransform;
    [SerializeField] private cReadyButtonController m_ReadyButtonController;
    [SerializeField] private cMenuNode m_MenuNode;
    [SerializeField] private cButton m_ReturnButton;

    public bool m_Active;

    private void Awake()
    {
        cLobbyManager.Instance.m_OnLobbyUpdate += () =>
        {
            UpdateUI(cLobbyManager.Instance.JoinedLobby);
        };
        
        m_MenuNode.OnActivateEvent.AddListener(EnableLobbyUI);
        m_ReturnButton.OnClickEvent.AddListener(LeaveLobby);
        
        m_ReadyButtonController.OnReadyStateChange += HandleReadyButtonStateChanged;
    }

    private void HandleReadyButtonStateChanged(bool isReady)
    {
        if(!m_Active) return;
        
        cLobbyManager.Instance.UpdateIsPlayerReadyRateLimited(isReady);
        UpdateUI(cLobbyManager.Instance.JoinedLobby);
    }

    public void EnableLobbyUI()
    {
        UpdateUI(cLobbyManager.Instance.JoinedLobby);
        m_Active = true;
    }
    
    public void LeaveLobby()
    {
        cLobbyManager.Instance.LeaveLobby();
        DisableLobbyUI();
    }
    
    public void DisableLobbyUI()
    {
        m_Active = false;
        m_ReadyButtonController.ResetState();
        m_MenuNode.Deactivate();
    }

    public void UpdateUI(Lobby lobby)
    {
        var isHost = cLobbyManager.Instance.IsHost;
        
        foreach (var VARIABLE in m_LayoutTransform.gameObject.GetChilds())
        {
            Destroy(VARIABLE.gameObject);
        }
        
        var lobbyName = lobby.Name;
        var playerCount = $"{lobby.Players.Count}/{lobby.MaxPlayers}";
        var gameMode = lobby.Data["GameMode"].Value;
        m_LobbyUnit.UpdateUI(lobbyName,playerCount,gameMode);

        foreach (var VARIABLE in lobby.Players)
        {
            var ins = Instantiate(m_PlayerUnit, m_LayoutTransform);

            bool isReady;
            bool kickButton;

            if (VARIABLE.Id ==  AuthenticationService.Instance.PlayerId)
            {
                isReady = m_ReadyButtonController.IsReady;
                kickButton = false;
            }
            else
            {
                isReady=VARIABLE.Data["IsReady"].Value == "True";

                kickButton = isHost;
            }
            
            ins.UpdateUI(VARIABLE.Data["PlayerName"].Value, int.Parse(VARIABLE.Data["IconIndex"].Value), VARIABLE, kickButton,isReady);
        }
    }
}
