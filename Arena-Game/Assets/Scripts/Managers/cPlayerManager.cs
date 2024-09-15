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
    
    public Action<Transform> m_OwnerPlayerSpawn= delegate {  };

    public List<GameObject> m_Players = new List<GameObject>();

    private int m_DeathPlayerCount;

    public bool CheckExistLastStandingPlayer()
    {
        m_DeathPlayerCount++;
        return m_DeathPlayerCount >= m_Players.Count - 1;
    }

    public void DestroyPlayers()
    {
        foreach (var VARIABLE in m_Players)
        {
            Destroy(VARIABLE);
        }
        m_Players.Clear();
    }

    public GameObject SpawnPlayer(Vector3 pos, Quaternion rot, ulong id)
    {
        var go = Instantiate(m_Player,pos, rot);
        go.GetComponent<NetworkObject>().SpawnAsPlayerObject(id);
        return go;
    }
}
