using System;
using System.Collections.Generic;
using DefaultNamespace;
using DemoBlast.Managers.SaveManager;
using DemoBlast.Utils;
using DG.Tweening;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum eGameMode
{
    PvE,
    PvP
}

public class cGameManager : cSingleton<cGameManager>
{
    [SerializeField] private cPlayerIconList m_PlayerIconList;
    [SerializeField] private cSaveManager m_InstanceSaveManager;
    [SerializeField] private NetworkObject m_NetworkObject;
    [SerializeField] private cHealthBar m_BossUIHealthBar;
    [SerializeField] private cHealthBar m_PlayerUIHealthBar;
    [SerializeField] private bool m_IsBossUIBeingUsed;
    [SerializeField] private bool m_IsPlayerUIBeingUsed;
    [SerializeField] private cPvPManager m_PvPManager;
    [SerializeField] private cPvEManager m_PvEManager;


    private IGameModeHandler m_GameModeHandler;
    private ISaveManager m_SaveManager;
    private int m_SpawnOffset;
    private eGameMode m_CurrentGameMode = eGameMode.PvE;
    
    public Transform m_OwnerPlayer;
    public int m_OwnerPlayerId;
    public Action m_OnNpcDied = delegate {  };
    public Action m_OnPlayerDied = delegate {  };
    public Action m_GameStarted = delegate {  };
    
    public cPlayerIconList PlayerIconList => m_PlayerIconList;
    public ISaveManager SaveManager
    {
        get
        {
            if (m_SaveManager == null)
            {
                m_SaveManager = m_InstanceSaveManager.GetComponent<ISaveManager>();
            }

            return m_SaveManager;
        }
    }

    public eGameMode CurrentGameMode
    {
        get => m_CurrentGameMode;
        set => m_CurrentGameMode = value;
    }

    public IGameModeHandler GameModeHandler => m_GameModeHandler;

    private void Start()
    {
        cPlayerManager.Instance.m_OwnerPlayerSpawn += transform1 =>
        {
            m_OwnerPlayerId = transform1.GetComponent<IDamagable>().TeamID;
            m_OwnerPlayer = transform1;
            CameraManager.Instance.OnPlayerSpawn();
        };
        NetworkManager.Singleton.OnClientConnectedCallback += obj =>
        {
         
        };

        m_BossUIHealthBar.m_OnVisibleUpdate += b =>
        {
            if (b == false)
            {
                m_IsBossUIBeingUsed = false;
            }
        };
        
        m_PlayerUIHealthBar.m_OnVisibleUpdate += b =>
        {
            if (b == false)
            {
                m_IsPlayerUIBeingUsed = false;
            }
        };

        DOVirtual.DelayedCall(2, () =>
        {
            cUIManager.Instance.HidePage(Page.Loading);
        });
    }

    public void StartGame()
    {
        switch (m_CurrentGameMode)
        {
            case eGameMode.PvE:
                m_GameModeHandler = m_PvEManager;
                break;
            case eGameMode.PvP:
                m_GameModeHandler = m_PvPManager;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        m_GameStarted.Invoke();
        GameModeHandler.StartGame();
    }

    // private async UniTask StartRound()
    // {
    //     await UniTask.WaitForSeconds(10);
    //     m_ProjectSceneManager.SpawnScene(cLevelSelectView.Instance.SelectedLevelUnit.LevelSo.SceneName);
    //     if (NetworkManager.Singleton.IsHost)
    //     {
    //         m_OnNpcDied = delegate {  };
    //         m_OnNpcDied += CheckSuccess;
    //     }
    // }
    
    private Scene loadedScene;

    private int clientCount;
    private void OnClientStarted()
    {
        clientCount++;
        Debug.Log($"client count {clientCount}");
    }

    private void OnLoaded(string scenename, LoadSceneMode loadscenemode, List<ulong> clientscompleted, List<ulong> clientstimedout)
    {
        Debug.Log("Loaded!!!!");
    }
    
    public cHealthBar GiveMeBossUIHealthBar()
    {
        if (m_IsBossUIBeingUsed)
        {
            return null;
        }
        else
        {
            m_IsBossUIBeingUsed = true;
            return m_BossUIHealthBar;
        }
    }
    
    public cHealthBar GiveMePlayerUIHealthBar()
    {
        if (m_IsPlayerUIBeingUsed)
        {
            return null;
        }
        else
        {
            m_IsPlayerUIBeingUsed = true;
            return m_PlayerUIHealthBar;
        }
    }
    
    public Action m_OnMainMenuButton = delegate {  };
    [SerializeField] private cGameManagerNetworkBehaviour m_GameManagerNetworkBehaviour;

    public void OnMainMenuButtonClick()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            m_GameManagerNetworkBehaviour.OnHostLeaveClientRpc();
            LeaveGame();
        }
        else
        {
            LeaveGame();
        }
    }
    
    public void LeaveGame()
    {
        NetworkManager.Singleton.Shutdown();
        m_OnMainMenuButton.Invoke();
    }
}