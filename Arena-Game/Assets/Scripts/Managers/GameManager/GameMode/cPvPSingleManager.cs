using System.Collections.Generic;
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
    
    private int m_SpawnOffset;
    private bool m_isActive;

    public void StartGame()
    {
        cGameManager.Instance.m_OnPlayerDied = delegate { };
        cGameManager.Instance.m_OnPlayerDied += CheckPvPSuccess;
            
        cGameManager.Instance.m_OnMainMenuButton += OnMainMenuButton;

        m_isActive = true;
            
        LoopStart();
    }

    private void LoopStart()
    {
        cUIManager.Instance.HidePage(Page.MainMenu);
        cUIManager.Instance.ShowPage(Page.Gameplay);
        cUIManager.Instance.ShowPage(Page.Loading);
        m_SpawnOffset = 0;
        cPlayerManager.Instance.DestroyPlayers();

        Transform player=null;
        
        foreach (var VARIABLE in NetworkManager.Singleton.ConnectedClients)
        {
            player = OnClientConnected(VARIABLE.Key).transform;
        }

        DOVirtual.DelayedCall(5, () =>
        {
            if (m_isActive)
            {
                cUIManager.Instance.HidePage(Page.Loading);
                CameraManager.Instance.FixLook();
                var enemyHuman = cNpcSpawner.Instance.EnemyHuman();
                var pos = new Vector3(0, 0, 5);
                var dir = Vector3.zero - pos;
                var lookRot = Quaternion.LookRotation(dir.normalized);
                enemyHuman.transform.rotation = lookRot;
                enemyHuman.transform.position = pos;

                if (player != null)
                {
                    enemyHuman.GetComponent<NPCHumanStateMachine>().m_enemies.Add(player.transform);
                }
            }
        });
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
        }
    }
    
    private void OnMainMenuButton()
    {
        if (m_isActive)
        {
            m_isActive = false;
            cGameManager.Instance.m_OnMainMenuButton -= OnMainMenuButton;
        }
    }
}