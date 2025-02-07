using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class cLobbyUnit : MonoBehaviour
{
    [SerializeField] private cLobbyUnitUI m_LobbyUnitUI;

    private Lobby m_Lobby;
    
    public void UpdateUI(Lobby lobby)
    {
        m_Lobby = lobby;
        var lobbyName = lobby.Name;
        var playerCount = $"{lobby.Players.Count}/{lobby.MaxPlayers}";
        var gameMode = lobby.Data["GameMode"].Value;
        m_LobbyUnitUI.UpdateUI(lobbyName,playerCount,gameMode);
    }

    public void OnClick()
    {
        cLobbyListUI.Instance.OnLobbySelected(m_Lobby);
    }
}