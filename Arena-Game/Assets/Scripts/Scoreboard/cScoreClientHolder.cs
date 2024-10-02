using System;
using System.Collections.Generic;
using DefaultNamespace;
using ArenaGame.Utils;
using Unity.Netcode;
using UnityEngine;

public class cScoreClientHolder : cSingleton<cScoreClientHolder>
{
    public cScoreboardController m_ScoreboardController;
    
    public Dictionary<ulong, cClientScoreController> m_ClientScoreUnitsDic =
        new Dictionary<ulong, cClientScoreController>();

    public cClientScoreController ClientScoreUnit { get; set; }

    public void AddDead(DamageWrapper damageWrapper)
    {
        ClientScoreUnit.DeadCount.Value++;
        m_ScoreboardController.AddKillServerRpc(damageWrapper.Instigator.CharacterNetworkController.OwnerClientId);
    }

    public void AddKillClientRpc(ulong ownerId)
    {
        Debug.Log($"{ownerId} {ClientScoreUnit.OwnerClientId}");
        if (ownerId == ClientScoreUnit.OwnerClientId)
        {
            ClientScoreUnit.KillCount.Value++;
        }
    }
    
    public void ClearDict()
    {
        m_ClientScoreUnitsDic.Clear();
    }
}