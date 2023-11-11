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

        var gameMode = cGameManager.Instance.CurrentGameMode;
        cLobbyCreationManager.Instance.OnCreate(new cLobbyCreationManager.LobbyCreationSettingWrapper()
        {
            m_LobbyName = m_LobbyNameInputAreaController.LobbyName, 
            m_PlayerCount = m_PlayerCountButton.PlayerCount, 
            m_IsPrivate = m_VisibilityButton.isPrivate, 
            m_GameMode = gameMode
        }, Created);
        gameObject.SetActive(false);
    }
}
