using System.Collections.Generic;
using DefaultNamespace;
using DemoBlast.Utils;
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
        m_ScoreboardController.AddKillServerRpc(damageWrapper.Character.CharacterNetworkController.OwnerClientId);
    }

    public void AddKillClientRpc(ulong ownerId)
    {
        Debug.Log($"{ownerId} {ClientScoreUnit.OwnerClientId}");
        if (ownerId == ClientScoreUnit.OwnerClientId)
        {
            ClientScoreUnit.KillCount.Value++;
        }
    }
}