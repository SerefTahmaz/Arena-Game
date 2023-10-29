using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cCreateLobbyUIController : MonoBehaviour
{
    [SerializeField] private cLobbyNameInputAreaController m_LobbyNameInputAreaController;
    [SerializeField] private cVisibilityButton m_VisibilityButton;
    [SerializeField] private cPlayerCountButton m_PlayerCountButton;
    [SerializeField] private cGameModeButton m_GameModeButton;

    public void OnCreate()
    {
        void Created()
        {
            cLobbyUI.Instance.EnableLobby();
        }
        
        cLobbyManager.Instance.CreateLobby(m_LobbyNameInputAreaController.LobbyName
            , m_PlayerCountButton.PlayerCount, 
            m_VisibilityButton.isPrivate, 
            m_GameModeButton.GameMode, 
            Created);
        
        
        gameObject.SetActive(false);
    }
}
