using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Managers.SaveManager;
using Unity.Netcode;
using UnityEngine;

public class MultiplayerNetworkHelper : NetworkBehaviour
{
    public NetworkVariable<int> m_MapIndex = new NetworkVariable<int>(0,NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);
    
    public NetworkVariable<bool> m_IsGameStarted = new NetworkVariable<bool>(false,NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);
    
    public NetworkVariable<bool> m_DrawSwordOnStart = new NetworkVariable<bool>(false,NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);
    
    public NetworkVariable<int> m_RewardExp = new NetworkVariable<int>(0,NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);
    
    public NetworkVariable<bool> m_IsMapLoaded = new NetworkVariable<bool>(false,NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);
    
    public NetworkVariable<int> m_NetworkPrefabRegisteredCount = new NetworkVariable<int>(0,NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);


    [ClientRpc]
    public void CheckGameEndClientRpc()
    {
        var playerSM = cGameManager.Instance.m_OwnerPlayer.GetComponent<cPlayerStateMachineV2>();
        if (playerSM.CurrentState != playerSM.Dead)
        {
            cGameManager.Instance.HandleWin();
        }
        else
        {
            cGameManager.Instance.HandleLose();
        }
    }

    [ClientRpc]
    public void HandleWinClientRpc()
    {
        cGameManager.Instance.HandleWin();
    }
    
    [ClientRpc]
    public void HandleLoseClientRpc()
    {
        cGameManager.Instance.HandleLose();
    }

    public void ResetState()
    {
        if(!IsHost) return;

        m_DrawSwordOnStart.Value = false;
        m_IsGameStarted.Value = false;
        m_RewardExp.Value = 0;
        m_IsMapLoaded.Value = false;
        m_NetworkPrefabRegisteredCount.Value = 0;
    }

    public void IncreaseSceneLoadedCount()
    {
        m_NetworkPrefabRegisteredCount.Value++;
    }
}
