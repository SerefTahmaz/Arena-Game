using System.Collections.Generic;
using DG.Tweening;
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
        cUIManager.Instance.ShowPage(Page.Loading);
        m_SpawnOffset = 0;
        cPlayerManager.Instance.DestroyPlayers();
        
        foreach (var VARIABLE in NetworkManager.Singleton.ConnectedClients)
        {
            OnClientConnected(VARIABLE.Key);
        }

        DOVirtual.DelayedCall(5, () =>
        {
            if (m_isActive)
            {
                cUIManager.Instance.HidePage(Page.Loading);
                cNpcSpawner.Instance.EnemyHuman();
            }
        });
    }
    
    private void OnClientConnected(ulong obj)
    {
        Vector3 pos;
        GameObject go;
        pos = (Vector3.right * Mathf.Cos(m_SpawnOffset*90) + Vector3.forward * Mathf.Sin(m_SpawnOffset*90))*5;
        Vector3 dir = Vector3.zero - pos;
        var lookRot = Quaternion.LookRotation(dir.normalized);
        go = cPlayerManager.Instance.SpawnPlayer(pos, lookRot, obj);
        go.GetComponent<cPlayerCharacter>().CharacterNetworkController.m_TeamId.Value = 10 + m_SpawnOffset;
        m_SpawnOffset++;
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