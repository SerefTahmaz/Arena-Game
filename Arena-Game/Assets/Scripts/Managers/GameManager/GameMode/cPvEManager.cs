using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArenaGame.Managers.SaveManager;
using DG.Tweening;
using FiniteStateMachine;
using Gameplay.Character.NPCHuman;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cPvEManager : MonoBehaviour,IGameModeHandler
{
    [SerializeField] private ProjectSceneManager m_ProjectSceneManager;
    [SerializeField] private bool m_NPCNonActiveAtStart; 
    
    private int m_SpawnOffset;
    private bool m_IsActive;

    public void StartGame()
    {
        SaveGameHandler.Load();
        var currentMap = SaveGameHandler.SaveData.m_CurrentMap;
        MapManager.instance.SetMap(currentMap);
        
        cGameManager.Instance.m_OnNpcDied = delegate { };
        cGameManager.Instance.m_OnNpcDied += CheckSuccess;
        
        cGameManager.Instance.m_OnPlayerDied = delegate { };
        cGameManager.Instance.m_OnPlayerDied += CheckSuccess;

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        cGameManager.Instance.m_OnMainMenuButton += OnMainMenuButton;
        
        m_IsActive = true;
            
        LoopStart();
    }

    private void LoopStart()
    {
        cUIManager.Instance.HidePage(Page.MainMenu);
        cUIManager.Instance.ShowPage(Page.Gameplay);
        cUIManager.Instance.ShowPage(Page.Loading);
        
        m_SpawnOffset = 0;
        cPlayerManager.Instance.DestroyPlayers();
        cNpcManager.Instance.DestroyNpcs();
        foreach (var VARIABLE in NetworkManager.Singleton.ConnectedClients)
        {
            OnClientConnected(VARIABLE.Key);
        }
        
        DOVirtual.DelayedCall(5, (() =>
        {
            if (m_IsActive)
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
        m_ProjectSceneManager.SpawnScene(PVELevelSelectView.Instance.SelectedLevelUnit.LevelSo.SceneName);
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadCompleted;
    }

    private void OnLoadCompleted(string scenename, LoadSceneMode loadscenemode, List<ulong> clientscompleted, List<ulong> clientstimedout)
    {
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= OnLoadCompleted;
        DOVirtual.DelayedCall(1, () =>
        {
            m_ProjectSceneManager.UnloadScene();
            MultiplayerLocalHelper.instance.NetworkHelper.m_IsGameStarted.Value = true;
            
            if (cGameManager.Instance.m_OwnerPlayer != null)
            {
                foreach (var npcIns in cNpcManager.Instance.m_Npcs)
                {
                    var npcSm = npcIns.GetComponentInChildren<cStateMachine>();
                    if(!m_NPCNonActiveAtStart) npcSm.m_enemies.Add( cGameManager.Instance.m_OwnerPlayer);
                }
            }
        });
    }

    private void CheckSuccess()
    {
        var players = FindObjectsOfType<cPlayerStateMachineV2>().Where((v2 => v2.CurrentState != v2.Dead));
        var isAllPlayersDead = !players.Any();
        
        if (cNpcManager.Instance.CheckIsAllNpcsDied())
        {
            PVELevelSelectView.Instance.SelectNext();
            
            OnGameEnd();
            MultiplayerLocalHelper.instance.NetworkHelper.HandleWinClientRpc();
        }
        else if (isAllPlayersDead)
        {
            OnGameEnd();
            MultiplayerLocalHelper.instance.NetworkHelper.HandleLoseClientRpc();
        }
    }
    
    private void OnMainMenuButton()
    {
        if (m_IsActive)
        {
            OnGameEnd();
        }
    }
    
    private void OnGameEnd()
    {
        m_IsActive = false;
        cGameManager.Instance.m_OnMainMenuButton -= OnMainMenuButton;
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        
        Debug.Log("GameEnded!!!!!!!!!!");

        //TODO: Find a clear method to reach npcs and functinality
        foreach (var VARIABLE in cNpcManager.Instance.m_Npcs)
        {
            VARIABLE.GetComponentInChildren<cStateMachine>().m_enemies.Clear();
        }
    }

}