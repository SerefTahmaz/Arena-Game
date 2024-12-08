using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Extensions.Unity.ImageLoader;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class cPlayerUnit : MonoBehaviour
{
    [SerializeField] private Image m_Icon;
    [SerializeField] private TMP_Text m_PlayerName;
    [SerializeField] private GameObject m_KickButton;
    [SerializeField] private GameObject m_IsReady;

    private Player m_Player;

    public async UniTask UpdateUI(string playerName, string ProfilePhotoURl, Player player, bool kickButton, bool isReady)
    {
        m_PlayerName.text = playerName;
        m_Player = player;
        m_KickButton.SetActive(kickButton);
        m_IsReady.SetActive(isReady);
        
        Debug.Log($"Profile url {ProfilePhotoURl}");

        if (!string.IsNullOrEmpty(ProfilePhotoURl))
        {
            var pp = await ImageLoader.LoadSprite(ProfilePhotoURl);
            if (m_Icon)
            {
                m_Icon.sprite = pp;
            }
        }
        else
        {
            m_Icon.sprite = PrefabList.Get().DefaultPPIcon;
        }
    }

    public void KickPlayer()
    {
        cLobbyManager.Instance.KickPlayer(m_Player.Id);
    }
}