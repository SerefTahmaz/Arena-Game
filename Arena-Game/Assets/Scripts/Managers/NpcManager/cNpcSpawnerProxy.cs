using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FiniteStateMachine;
using Unity.Netcode;
using UnityEngine;

public class cNpcSpawnerProxy : NetworkBehaviour
{
    [SerializeField] private GameObject m_NetworkPrefab;
    [SerializeField] private Transform m_SpawnPoint;
    [SerializeField] private cHealthManager.eHealthBarState m_HealthBarState;
    [SerializeField] private bool m_SetTeamId;
    [SerializeField] private int m_TeamId;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        SpawnNpc();
    }

    public void SpawnNpc()
    {
        if (IsHost)
        {
            NetworkManager.Singleton.AddNetworkPrefab(m_NetworkPrefab);
            cNpcManager.Instance.AddToBeRemovedAtEndNetworkPrefab(m_NetworkPrefab);
            
            GameObject go = Instantiate(m_NetworkPrefab, m_SpawnPoint.position, m_SpawnPoint.rotation);
            go.GetComponent<NetworkObject>().Spawn();
            foreach (var VARIABLE in  go.GetComponentsInChildren<cHealthManager>())
            {
                VARIABLE.HealthBarState = m_HealthBarState;
            }

            if (m_SetTeamId)
            {
                var character = go.GetComponentInChildren<cCharacterNetworkController>();
                character.m_TeamId.Value = m_TeamId;
            }
           
            cNpcManager.Instance.m_Npcs.Add(go);
            
            // go.GetComponentInChildren<cStateMachine>().m_enemies.AddRange(FindObjectsOfType<cPlayerStateMachineV2>().Select((v2 => v2.transform)).ToList());
        }
        gameObject.SetActive(false);
    }
}