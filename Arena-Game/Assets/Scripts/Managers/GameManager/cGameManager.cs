using System;
using System.Collections.Generic;
using DefaultNamespace;
using ArenaGame.Managers.SaveManager;
using ArenaGame.Utils;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum eGameMode
{
    PvE,
    PvP,
    PvPSingle,
    Freeroam
}

public class cGameManager : cSingleton<cGameManager>
{
    [SerializeField] private cPlayerIconList m_PlayerIconList;
    [SerializeField] private SaveManager m_InstanceSaveManager;
    [SerializeField] private NetworkObject m_NetworkObject;
    [SerializeField] private cHealthBar m_BossUIHealthBar;
    [SerializeField] private cHealthBar m_PlayerUIHealthBar;
    [SerializeField] private bool m_IsBossUIBeingUsed;
    [SerializeField] private bool m_IsPlayerUIBeingUsed;
    [SerializeField] private cPvPManager m_PvPManager;
    [SerializeField] private cPvEManager m_PvEManager;
    [SerializeField] private cPvPSingleManager m_CPvPSingleManager;
    [SerializeField] private FreeroamGameMode m_FreeroamGameMode;

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

    public bool IsGameplayActive { get; set; }

    private bool m_IsServerDisconnectedClient;
    private bool m_IsServerDisconnectedItself;

    private void Start()
    {
        cPlayerManager.Instance.m_OwnerPlayerSpawn += transform1 =>
        {
            m_OwnerPlayerId = transform1.GetComponent<IDamagable>().TeamID;
            m_OwnerPlayer = transform1;
        };

        HandleDisconnectionStates();

        DOVirtual.DelayedCall(2, () =>
        {
            InternetManager.Instance.SetCheckInternetConnection(true);
        },false);

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

        cUIManager.Instance.ShowPage(Page.Loading,this);
        //TODO: Hide when loading completes!
        DOVirtual.DelayedCall(2, () =>
        {
            cUIManager.Instance.HidePage(Page.Loading,this);
            cUIManager.Instance.ShowPage(Page.StartMenu,this,true);
        });
    }

    private void HandleDisconnectionStates()
    {
        //Called on the owner only
        NetworkManager.Singleton.OnClientStopped += b =>
        {
            if (IsGameplayActive)
            {
                GameEnd();
                var insDisconnectedPopUpController = GlobalFactory.DisconnectedPopUpFactory.Create();
                insDisconnectedPopUpController.Init("Disconnected from the server");

                if (NetworkManager.Singleton.IsClient)
                {
                    DOVirtual.DelayedCall(0.5f, () =>
                    {
                        if (!m_IsServerDisconnectedClient)
                        {
                            SetPlayerDisqualified();
                        }
                        insDisconnectedPopUpController.ActivateButton();
                    });
                    Debug.Log("Client Stopped");
                }
                else if (NetworkManager.Singleton.IsHost)
                {
                    insDisconnectedPopUpController.ActivateButton();
                    SetPlayerDisqualified();
                    m_IsServerDisconnectedItself = true;
                    Debug.Log("Host Stopped");
                }
            }
            
            Debug.Log("OnClientStopped");
        };

        NetworkManager.Singleton.OnClientDisconnectCallback += obj =>
        {
            // if(!NetworkManager.Singleton.IsHost) return;
            
            Debug.Log($"ID {obj}  hostid {NetworkManager.Singleton.LocalClientId}");
            // if(NetworkManager.Singleton.IsHost) Debug.Log($"connected count {NetworkManager.Singleton.ConnectedClientsList.Count}");
            
            if (IsGameplayActive)
            {
                if (NetworkManager.Singleton.IsHost && NetworkManager.Singleton.ConnectedClientsList.Count == 2)
                {
                    DOVirtual.DelayedCall(0.5f, () =>
                    {
                        if (!m_IsServerDisconnectedItself)
                        {
                            GameEnd();
                            var insDisconnectedPopUpController = GlobalFactory.DisconnectedPopUpFactory.Create();
                            insDisconnectedPopUpController.Init("Opponent disconnected from the server");
                            insDisconnectedPopUpController.ActivateButton();
                        }
                    },false);
                }

                if (NetworkManager.Singleton.IsClient && obj == 0)
                {
                    // Debug.Log("Not disqualified. Server remove it");
                    m_IsServerDisconnectedClient = true;
                }
                else
                {
                    // Debug.Log("Disqualified. Client remove itself");
                }
            }
            
            Debug.Log("OnClientDisconnectCallback");
        };
    }

    private void OnApplicationQuit()
    {
        if (IsGameplayActive)
        {
            SetPlayerDisqualified();
        }
    }

    public void HandleNoInternet()
    {
        if (IsGameplayActive)
        {
            NetworkManager.Singleton.Shutdown();
            Debug.Log("No internet shutdown");
            //GameEnd();
        }
    }

    public void SetPlayerDisqualified()
    {
        Debug.Log("Disqualified!!!");
        SaveGameHandler.Load();
        SaveGameHandler.SaveData.m_IsPlayerDisqualified = true;
        SaveGameHandler.Save();
    }

    public void StartGame()
    {
        m_IsBossUIBeingUsed = false;
        m_IsPlayerUIBeingUsed = false;
        
        switch (m_CurrentGameMode)
        {
            case eGameMode.PvE:
                m_GameModeHandler = m_PvEManager;
                break;
            case eGameMode.PvP:
                m_GameModeHandler = m_PvPManager;
                break;
            case eGameMode.PvPSingle:
                m_GameModeHandler = m_CPvPSingleManager;
                break;
            case eGameMode.Freeroam:
                m_GameModeHandler = m_FreeroamGameMode;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        m_GameStarted.Invoke();
        GameModeHandler.StartGame();

        IsGameplayActive = true;
        m_IsServerDisconnectedClient = false;
        m_IsServerDisconnectedItself = false;
    }
    
    public void StartGameClient()
    {
        IsGameplayActive = true;
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
            // m_GameManagerNetworkBehaviour.OnHostLeaveClientRpc();
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
        cLobbyManager.Instance.UpdateIsPlayerReadyRateLimited(false);
        
        //Clean up
        cNpcManager.Instance.DestroyNpcs();
        cPlayerManager.Instance.DestroyPlayers();
        
        cUIManager.Instance.ShowPage(Page.StartMenu,this);
        
        m_OnMainMenuButton.Invoke();
        
        cNpcManager.Instance.RemovePrefabsFromNetwork();
        
        Debug.Log("Called leave");
    }

    public async UniTask HandleWin()
    {
        await GameEnd(); 
        cUIManager.Instance.ShowPage(Page.Win,this);
    }

    public void HandleWinContinueButton()
    {
        cUIManager.Instance.HidePage(Page.Win,this);
        LeaveGame();
    }

    public async UniTask HandleLose()
    {
        await GameEnd();
        cUIManager.Instance.ShowPage(Page.Lose,this);
    }

    public async UniTask HandleFreeroamEnd()
    {
        StopGame();
        LeaveGame();
    }

    private async UniTask GameEnd()
    {
        StopGame();
        await UniTask.WaitForSeconds(2);
    }

    private void StopGame()
    {
        IsGameplayActive = false;
        SetInput(false);
    }

    public void SetInput(bool value)
    {
        InputManager.Instance.SetInput(value);
        CameraManager.Instance.SetInput(value);
    }

    public void HandleLoseContinueButton()
    {
        cUIManager.Instance.HidePage(Page.Lose,this);
        LeaveGame();
    }

    public void HandleStartingRelay()
    {
        cUIManager.Instance.HidePage(Page.StartMenu,this);
    }
}