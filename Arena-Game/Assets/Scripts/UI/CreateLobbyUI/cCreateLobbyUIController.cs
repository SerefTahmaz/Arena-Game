using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class cCreateLobbyUIController : MonoBehaviour
{
    [SerializeField] private cLobbyNameInputAreaController m_LobbyNameInputAreaController;
    [SerializeField] private cVisibilityButton m_VisibilityButton;
    [SerializeField] private cPlayerCountButton m_PlayerCountButton;
    [SerializeField] private cGameModeButton m_GameModeButton;
    [SerializeField] private cButton m_CreateButton;
    [SerializeField] private UnityEvent m_OnCreated;

    private void Awake()
    {
        m_CreateButton.OnClickEvent.AddListener(HandleCreateButtonClicked);
    }

    private void HandleCreateButtonClicked()
    {
        OnCreate();
    }

    public async UniTask OnCreate()
    {
        m_CreateButton.DeActivate();
        var gameMode = cGameManager.Instance.CurrentGameMode;
        var result = await cLobbyCreationManager.Instance.OnCreate(new cLobbyCreationManager.LobbyCreationSettingWrapper()
        {
            m_LobbyName = m_LobbyNameInputAreaController.LobbyName, 
            m_PlayerCount = m_PlayerCountButton.PlayerCount, 
            m_IsPrivate = m_VisibilityButton.isPrivate, 
            m_GameMode = gameMode
        });
        switch (result)
        {
            case RequestResult.Failed:
                break;
            case RequestResult.Success:
                m_OnCreated.Invoke();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        m_CreateButton.Activate();
    }
}
