using System.Collections.Generic;
using System.Linq;
using ArenaGame.Managers.SaveManager;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Character;
using Gameplay.Character.NPCHuman;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cPvPSingleManager : MonoBehaviour,IGameModeHandler
{
    [SerializeField] private ProjectSceneManager m_ProjectSceneManager;
    [SerializeField] private string m_NpcScene;
    [SerializeField] private bool m_NPCNonActiveAtStart; 
    
    private bool m_IsActive;

    private NPCHumanStateMachine m_NPCHumanStateMachine;
    
    private int m_SpawnOffset;

    public void StartGame()
    {
        cGameManager.Instance.StartGameClient(true);
        
        cGameManager.Instance.m_OnPlayerDied = delegate { };
        cGameManager.Instance.m_OnPlayerDied += HandlePlayerDied;
            
        cGameManager.Instance.m_OnMainMenuButton += OnMainMenuButton;

        m_IsActive = true;
            
        LoopStart();
    }

    private async UniTask LoopStart()
    {
        cUIManager.Instance.ShowPage(Page.Loading,this);
        var insMatchMaking = MatchMakingController.CreateInstanceMatchMaking();
        await insMatchMaking.Init();
        
        m_SpawnOffset = 0;
        cPlayerManager.Instance.DestroyPlayers();
        
        SaveGameHandler.Load();
        var currentMap = SaveGameHandler.SaveData.m_CurrentMap;
        await MapManager.Instance.SetMap(currentMap);
        
        Transform player=null;
        
        foreach (var VARIABLE in NetworkManager.Singleton.ConnectedClients)
        {
            player = OnClientConnected(VARIABLE.Key).transform;
        }

        cUIManager.Instance.HidePage(Page.Loading,this);

        if (m_IsActive)
        {
            MultiplayerLocalHelper.Instance.NetworkHelper.m_IsGameStarted.Value = true;
                
            var enemyHuman = cNpcSpawner.Instance.EnemyHuman();
            var pos = new Vector3(0, 0, 5);
            var dir = Vector3.zero - pos;
            var lookRot = Quaternion.LookRotation(dir.normalized);
            enemyHuman.transform.rotation = lookRot;
            enemyHuman.transform.position = pos;

            if (cGameManager.Instance.m_OwnerPlayer != null)
            {
                m_NPCHumanStateMachine = enemyHuman.GetComponent<NPCHumanStateMachine>();
                if(!m_NPCNonActiveAtStart)  m_NPCHumanStateMachine.NpcTargetHelper.IsAggressive = true;
            }
        } 
    }
    
    private GameObject OnClientConnected(ulong obj)
    {
        Vector3 pos;
        GameObject go;
        pos = new Vector3(0, 0, -5);
        Vector3 dir = Vector3.zero - pos;
        var lookRot = Quaternion.LookRotation(dir.normalized);
        go = cPlayerManager.Instance.SpawnPlayer(pos, lookRot, obj);
        go.GetComponent<HumanCharacter>().CharacterNetworkController.m_TeamId.Value = 10 + m_SpawnOffset;
        m_SpawnOffset++;
        return go;
    }
    
    private void HandlePlayerDied()
    {
        cPlayerManager.Instance.PlayerDied();
        CheckPvPSuccess();
    }

    private void CheckPvPSuccess()
    {
        if (cPlayerManager.Instance.CheckExistLastStandingPlayer())
        {
            // DOVirtual.DelayedCall(5, () =>
            // {
            //     if (m_isActive)
            //     {
            //         LoopStart();
            //     }
            // });
            Debug.Log($"Game Ended");
            var playerSM = cGameManager.Instance.m_OwnerPlayer.GetComponent<cPlayerStateMachineV2>();
            if (playerSM.CurrentState != playerSM.Dead)
            {
                OnGameEnd();
                cGameManager.Instance.HandleWin();
            }
            else
            {
                OnGameEnd();
                cGameManager.Instance.HandleLose();
                if (m_NPCHumanStateMachine != null)
                {
                    m_NPCHumanStateMachine.m_enemies.Clear();
                    m_NPCHumanStateMachine = null;
                }
            }
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
        Debug.Log("GameEnded!!!!!!!!!!");
    }
}