using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cPvEManager : MonoBehaviour,IGameModeHandler
{
    [SerializeField] private ProjectSceneManager m_ProjectSceneManager;
    
    private int m_SpawnOffset;
    
    private Action m_OnGameStart = delegate {  };
    private Action m_OnGameEnd = delegate {  };

    public Action OnGameEnd
    {
        get => m_OnGameEnd;
        set => m_OnGameEnd = value;
    }

    public Action OnGameStart
    {
        get => m_OnGameStart;
        set => m_OnGameStart = value;
    }

    public void StartGame()
    {
        cGameManager.Instance.m_OnNpcDied = delegate { };
        cGameManager.Instance.m_OnNpcDied += CheckSuccess;
        
        m_SpawnOffset = 0;
        cPlayerManager.Instance.DestroyPlayers();
        foreach (var VARIABLE in NetworkManager.Singleton.ConnectedClients)
        {
            Vector3 pos;
            pos = m_SpawnOffset*2 * Vector3.right;
            cPlayerManager.Instance.SpawnPlayer(pos, Quaternion.identity,VARIABLE.Key);
            
            m_SpawnOffset++;
        }

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        OnGameStart.Invoke();
            
        DOVirtual.DelayedCall(5, (() =>
        {
            StartNextGame();
        }));
    }

    private void OnClientConnected(ulong obj)
    {
        Vector3 pos;
        pos = m_SpawnOffset*2 * Vector3.right;
        cPlayerManager.Instance.SpawnPlayer(pos, Quaternion.identity,obj);
                    
        m_SpawnOffset++;
    }

    public void StartNextGame()
    {
        m_ProjectSceneManager.SpawnScene(cLevelSelectView.Instance.SelectedLevelUnit.LevelSo.SceneName);
    }

    private void CheckSuccess()
    {
        if (cNpcManager.Instance.CheckIsAllNpcsDied())
        {
            DOVirtual.DelayedCall(5, ContinueButton);
        }
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
        StartNextGame();
    }

    // public void MainMenuButton()
    // {
    //     UnloadLevel();
    //     NetworkManager.Singleton.SceneManager.OnUnloadEventCompleted += OnUnloadCompletedMainMenu;
    // }
    //
    // private void OnUnloadCompletedMainMenu(string scenename, LoadSceneMode loadscenemode, List<ulong> clientscompleted, List<ulong> clientstimedout)
    // {
    //     NetworkManager.Singleton.SceneManager.OnUnloadEventCompleted -= OnUnloadCompletedMainMenu;
    //     NetworkManager.Singleton.Shutdown();
    // }
}