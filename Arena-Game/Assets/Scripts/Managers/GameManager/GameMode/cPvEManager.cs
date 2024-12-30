using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArenaGame.Managers.SaveManager;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using FiniteStateMachine;
using Gameplay.Character.NPCHuman;
using UI.EndScreen;
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
        cGameManager.Instance.StartGameClient(true);
        
        cGameManager.Instance.m_OnNpcDied = delegate { };
        cGameManager.Instance.m_OnNpcDied += CheckSuccess;
        
        cGameManager.Instance.m_OnPlayerDied = delegate { };
        cGameManager.Instance.m_OnPlayerDied += HandlePlayerDied;

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        cGameManager.Instance.m_OnMainMenuButton += OnMainMenuButton;
        
        WinScreenUIController.Instance.RewardExp  = PVELevelSelectView.Instance.SelectedLevelUnit.LevelSo.ExpReward;
        
        m_IsActive = true;
            
        LoopStart();
    }

    private async UniTask LoopStart()
    {
        cUIManager.Instance.ShowPage(Page.Gameplay,this, true);
        LoadingScreen.Instance.ShowPage(this);
        
        UserSaveHandler.Load();
        var currentMap = UserSaveHandler.SaveData.m_CurrentMap;
        await MapManager.Instance.SetMapNetwork(currentMap); 
        
        m_SpawnOffset = 0;
        cPlayerManager.Instance.DestroyPlayers();
        cNpcManager.Instance.DestroyNpcs();
        foreach (var VARIABLE in NetworkManager.Singleton.ConnectedClients)
        {
            OnClientConnected(VARIABLE.Key);
        }
        
        //TODO: Start it when all clients ready
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
            LoadingScreen.Instance.HidePage(this);
            MultiplayerLocalHelper.Instance.SetGameStarted(true);
            
            if (cGameManager.Instance.m_OwnerPlayer != null)
            {
                foreach (var npcIns in cNpcManager.Instance.m_Npcs)
                {
                    var npcSm = npcIns.GetComponentInChildren<cCharacterStateMachine>();
                    if (!m_NPCNonActiveAtStart)
                    {
                        npcSm.NpcTargetHelper.IsAggressive = true;
                    }
                }
            }
        });
    }
    
    private void HandlePlayerDied()
    {
        cPlayerManager.Instance.PlayerDied();
        CheckSuccess();
    }

    private void CheckSuccess()
    {
        if(!m_IsActive) return;
        
        if (cNpcManager.Instance.CheckIsAllNpcsDied())
        {
            PVELevelSelectView.Instance.SelectNext();
            
            OnGameEnd();
            MultiplayerLocalHelper.Instance.NetworkHelper.HandleWinClientRpc();
        }
        else if (cPlayerManager.Instance.IsAllPlayersDead())
        {
            OnGameEnd();
            MultiplayerLocalHelper.Instance.NetworkHelper.HandleLoseClientRpc();
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
        
        cUIManager.Instance.HidePage(Page.Gameplay,this);
    }

}