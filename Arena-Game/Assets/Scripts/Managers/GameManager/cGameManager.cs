using System;
using System.Collections.Generic;
using DefaultNamespace;
using ArenaGame.Managers.SaveManager;
using ArenaGame.Utils;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Extensions.Unity.ImageLoader;
using Gameplay.Character;
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
    
    [SerializeField] private cPvPManager m_PvPManager;
    [SerializeField] private cPvEManager m_PvEManager;
    [SerializeField] private cPvPSingleManager m_CPvPSingleManager;
    [SerializeField] private FreeroamGameMode m_FreeroamGameMode;
    [SerializeField] private Sprite m_DefaultPP;

    private IGameModeHandler m_GameModeHandler;
    private eGameMode m_CurrentGameMode = eGameMode.PvE;
    private bool m_IsServerDisconnectedClient;
    private bool m_IsServerDisconnectedItself;
    
    public HumanCharacter m_OwnerPlayer;
    public int m_OwnerPlayerId;
    public Action m_OnNpcDied = delegate {  };
    public Action m_OnPlayerDied = delegate {  };
    public Action m_GameStarted = delegate {  };
    public Action m_GameEnded = delegate {  };
    public Action OnPlayerLose { get; set; }
    public Action OnPlayerWin { get; set; }
    
    public cPlayerIconList PlayerIconList => m_PlayerIconList;

    public eGameMode CurrentGameMode
    {
        get => m_CurrentGameMode;
        set => m_CurrentGameMode = value;
    }

    public IGameModeHandler GameModeHandler => m_GameModeHandler;

    public bool IsGameplayActive
    {
        get => m_IsGameplayActive;
        set => m_IsGameplayActive = value;
    }

    public bool IsOnlineGameplayActive
    {
        get => m_IsOnlineGameplayActive;
        set => m_IsOnlineGameplayActive = value;
    }

    private void Awake()
    {
        ImageLoader.Init();
    }

    private void Start()
    {
        DOVirtual.DelayedCall(2, () =>
        {
            InternetManager.Instance.SetCheckInternetConnection(true);
        },false);
        
        HandleDisconnectionStates();
        
        cPlayerManager.Instance.m_OwnerPlayerSpawn += transform1 =>
        {
            m_OwnerPlayerId = transform1.GetComponent<cCharacterNetworkController>().m_TeamId.Value;
            m_OwnerPlayer = transform1;
        };
    }

    public async UniTask StartMainScene()
    {
        LoadingScreen.Instance.ShowPage(this,true);
        await SceneManager.LoadSceneAsync("TempMain",LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("TempMain"));
        LoadingScreen.Instance.HidePage(this);
    }

    private void HandleDisconnectionStates()
    {
        //Called on the owner only
        NetworkManager.Singleton.OnClientStopped += b =>
        {
            if (IsGameplayActive)
            {
                StopGame();
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
                            StopGame();
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
        if (IsOnlineGameplayActive)
        {
            SetPlayerDisqualified();
        }
    }
    
    public void SetClosedAppInGameplay(bool value)
    {
        Debug.Log($"Closed App In Gameplay {value}");
        SaveGameHandler.SaveData.m_IsPlayerClosedAppInGameplay = value;
        SaveGameHandler.Save();
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
        SaveGameHandler.SaveData.m_IsPlayerDisqualified = true;
        SaveGameHandler.Save();
    }

    public void StartGame()
    {
        GameHealthBarManager.Instance.ResetStates();
        
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
        GameModeHandler.StartGame();
        
        m_IsServerDisconnectedClient = false;
        m_IsServerDisconnectedItself = false;
    }
    
    public void StartGameClient(bool isOnline = false)
    {
        m_GameStarted.Invoke();
        IsGameplayActive = true;
        IsOnlineGameplayActive = isOnline;
        cUIManager.Instance.ShowPage(Page.Gameplay,this);
    }
    
    public Action m_OnMainMenuButton = delegate {  };
    [SerializeField] private cGameManagerNetworkBehaviour m_GameManagerNetworkBehaviour;
    [SerializeField] private bool m_IsOnlineGameplayActive;
    [SerializeField] private bool m_IsGameplayActive;

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
        m_GameEnded.Invoke();
        
        cNpcManager.Instance.RemovePrefabsFromNetwork();
        
        Debug.Log("Called leave");
    }

    public async UniTask HandleWin()
    {
        await GameEnd(); 
        OnPlayerWin?.Invoke();
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
        OnPlayerLose?.Invoke();
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
        cUIManager.Instance.HidePage(Page.Gameplay,this);
        IsGameplayActive = false;
        IsOnlineGameplayActive = false;
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

    public async UniTask HandleSighOut()
    {
        LoadingScreen.Instance.ShowPage(this);
        await MapManager.Instance.RemoveCurrentLevel();
        await MapManager.Instance.UnloadFreeroam();
        await SceneManager.UnloadSceneAsync("TempMain");
        LoadingScreen.Instance.HidePage(this);
        AuthManager.Instance.SignOut();
    }
}