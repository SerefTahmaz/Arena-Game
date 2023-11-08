using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using DemoBlast.Managers.SaveManager;
using DemoBlast.Utils;
using DG.Tweening;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cGameManager : cSingleton<cGameManager>
{
    [SerializeField] private cPlayerIconList m_PlayerIconList;
    [SerializeField] private cSaveManager m_InstanceSaveManager;
    [SerializeField] private ProjectSceneManager m_ProjectSceneManager;
    [SerializeField] private NetworkObject m_NetworkObject;
    [SerializeField] private GameObject m_Player;
    [SerializeField] private cHealthBar m_BossUIHealthBar;
    [SerializeField] private cHealthBar m_PlayerUIHealthBar;
    [SerializeField] private bool m_IsBossUIBeingUsed;
    [SerializeField] private bool m_IsPlayerUIBeingUsed;

    private ISaveManager m_SaveManager;
    private int m_SpawnOffset;
    private eGameMode m_CurrentGameMode;
    
    public Transform m_OwnerPlayer;
    public int m_OwnerPlayerId;
    public Action m_OnNpcDied = delegate {  };
    
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

    public enum eGameMode
    {
        PvE,
        PvP
    }

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
            if (NetworkManager.Singleton.IsHost)
            {
                var pos = m_SpawnOffset*2 * Vector3.right;
                var go = Instantiate(m_Player,pos, Quaternion.identity);
                go.GetComponent<NetworkObject>().SpawnAsPlayerObject(obj);
                m_SpawnOffset++;
            }
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
    }

    public void StartGame()
    {
        StartRound();
    }

    private void StartRound()
    {
        DOVirtual.DelayedCall(10, () =>
        {
            Debug.Log("CALLLEEDDD!!!");
            m_ProjectSceneManager.SpawnScene(cLevelSelectView.Instance.SelectedLevelUnit.LevelSo.SceneName);
            if (NetworkManager.Singleton.IsHost)
            {
                m_OnNpcDied = delegate { };
                m_OnNpcDied += CheckSuccess;
            }
        });
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

    private void CheckSuccess()
    {
        if (cNpcManager.Instance.CheckIsAllNpcsDied())
        {
            DOVirtual.DelayedCall(5, ContinueButton);
        }
    }

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
    
    public void UnloadLevel()
    {
        cNpcManager.Instance.DestroyNpcs();
        m_ProjectSceneManager.UnloadScene();
    }

    public void ContinueButton()
    {
        UnloadLevel();
        NetworkManager.Singleton.SceneManager.OnUnloadEventCompleted += OnUnloadCompletedContinueButton;
    }
    
    private void OnUnloadCompletedContinueButton(string scenename, LoadSceneMode loadscenemode, List<ulong> clientscompleted, List<ulong> clientstimedout)
    {
        NetworkManager.Singleton.SceneManager.OnUnloadEventCompleted -= OnUnloadCompletedContinueButton;
        cLevelSelectView.Instance.SelectNext();
        StartRound();
    }

    public void MainMenuButton()
    {
        UnloadLevel();
        NetworkManager.Singleton.SceneManager.OnUnloadEventCompleted += OnUnloadCompletedMainMenu;
    }

    private void OnUnloadCompletedMainMenu(string scenename, LoadSceneMode loadscenemode, List<ulong> clientscompleted, List<ulong> clientstimedout)
    {
        NetworkManager.Singleton.SceneManager.OnUnloadEventCompleted -= OnUnloadCompletedMainMenu;
        NetworkManager.Singleton.Shutdown();
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
}