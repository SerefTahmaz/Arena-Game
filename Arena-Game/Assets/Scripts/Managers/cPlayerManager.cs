using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using ArenaGame.Utils;
using FiniteStateMachine;
using Unity.Netcode;
using UnityEngine;

public class cPlayerManager : cSingleton<cPlayerManager>
{
    [SerializeField] private GameObject m_Player;
    
    public Action<cCharacter> m_OwnerPlayerSpawn= delegate {  };

    public List<GameObject> m_Players = new List<GameObject>();

    private int m_DeathPlayerCount;

    public bool CheckExistLastStandingPlayer()
    {
        return m_DeathPlayerCount >= m_Players.Count - 1;
    }

    public bool IsAllPlayersDead()
    {
        return m_DeathPlayerCount >= m_Players.Count;
    }

    public void DestroyPlayers()
    {
        foreach (var VARIABLE in m_Players)
        {
            Destroy(VARIABLE);
        }
        m_Players.Clear();
        m_DeathPlayerCount = 0;
    }

    public GameObject SpawnPlayer(Vector3 pos, Quaternion rot, ulong id)
    {
        var go = Instantiate(m_Player,pos, rot);
        go.GetComponent<NetworkObject>().SpawnAsPlayerObject(id);
        return go;
    }

    public void PlayerDied()
    {
        m_DeathPlayerCount++;
    }
}
