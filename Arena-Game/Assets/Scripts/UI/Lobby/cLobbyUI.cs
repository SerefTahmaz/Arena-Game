using System;
using System.Collections;
using System.Collections.Generic;
using DemoBlast.UI;
using DemoBlast.Utils;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Events;

public class cLobbyUI : cSingleton<cLobbyUI>
{
    [SerializeField] private cLobbyUnitUI m_LobbyUnit;
    [SerializeField] private cPlayerUnit m_PlayerUnit;
    [SerializeField] private Transform m_LayoutTransform;
    [SerializeField] private cView m_View;
    [SerializeField] private UnityEvent m_OnDisableLobby;

    public bool m_Active => m_View.m_IsActive;

    private void Awake()
    {
        cLobbyManager.Instance.m_OnLobbyUpdate += () =>
        {
            UpdateUI(cLobbyManager.Instance.JoinedLobby);
        };
    }

    public void EnableLobby()
    {
        m_View.Activate(true);
        UpdateUI(cLobbyManager.Instance.JoinedLobby);
    }
    
    public void DisableLobby()
    {
        m_View.Deactivate(true);
    }
    
    public void DisableLobbyUI()
    {
        m_OnDisableLobby.Invoke();
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
            ins.UpdateUI(VARIABLE.Data["PlayerName"].Value, int.Parse(VARIABLE.Data["IconIndex"].Value), VARIABLE, isHost);
        }
    }
}
