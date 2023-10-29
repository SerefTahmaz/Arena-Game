using System;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class cPlayerUnit : MonoBehaviour
{
    [SerializeField] private Image m_Icon;
    [SerializeField] private TMP_Text m_PlayerName;
    [SerializeField] private GameObject m_KickButton;

    private Player m_Player;
    
    public void UpdateUI(string playerName, int iconIndex, Player player, bool isHost)
    {
        m_PlayerName.text = playerName;
        m_Icon.sprite = cGameManager.Instance.PlayerIconList.PlayerIcons[iconIndex].Icon;
        m_Player = player;
        m_KickButton.SetActive(isHost);
    }

    public void KickPlayer()
    {
        cLobbyManager.Instance.KickPlayer(m_Player.Id);
    }
}