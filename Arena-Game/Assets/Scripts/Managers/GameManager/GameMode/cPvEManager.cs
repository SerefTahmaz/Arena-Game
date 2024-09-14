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
    private bool m_isActive;

    private void OnMainMenuButton()
    {
        if (m_isActive)
        {
            m_isActive = false;
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            cGameManager.Instance.m_OnMainMenuButton -= OnMainMenuButton;
        }
    }

    public void StartGame()
    {
        cGameManager.Instance.m_OnNpcDied = delegate { };
        cGameManager.Instance.m_OnNpcDied += CheckSuccess;

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;

        m_isActive = true;
        
        cGameManager.Instance.m_OnMainMenuButton += OnMainMenuButton;
            
        LoopStart();
    }

    private void LoopStart()
    {
        cUIManager.Instance.HidePage(Page.MainMenu);
        cUIManager.Instance.ShowPage(Page.Gameplay);
        cUIManager.Instance.ShowPage(Page.Loading);
        
        m_SpawnOffset = 0;
        cPlayerManager.Instance.DestroyPlayers();
        foreach (var VARIABLE in NetworkManager.Singleton.ConnectedClients)
        {
            Vector3 pos;
            pos = m_SpawnOffset*2 * Vector3.right;
            cPlayerManager.Instance.SpawnPlayer(pos, Quaternion.identity,VARIABLE.Key);
            
            m_SpawnOffset++;
        }
        
        DOVirtual.DelayedCall(5, (() =>
        {
            if (m_isActive)
            {
                StartNextGame();
            }
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
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadCompleted;
    }

    private void OnLoadCompleted(string scenename, LoadSceneMode loadscenemode, List<ulong> clientscompleted, List<ulong> clientstimedout)
    {
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= OnLoadCompleted;
        DOVirtual.DelayedCall(1, () =>
        {
            m_ProjectSceneManager.UnloadScene();
            cUIManager.Instance.HidePage(Page.Loading);
        });
    }

    private void CheckSuccess()
    {
        if (cNpcManager.Instance.CheckIsAllNpcsDied())
        {
            DOVirtual.DelayedCall(5, (() =>
            {
                if (m_isActive)
                {
                    cLevelSelectView.Instance.SelectNext();
                    cNpcManager.Instance.DestroyNpcs();
                    LoopStart();
                }
            }));
        }
    }

}