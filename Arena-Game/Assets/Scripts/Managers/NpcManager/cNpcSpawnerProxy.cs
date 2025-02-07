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
        if (!cNpcManager.Instance.ContainsToBeRemovedAtEndNetworkPrefab(m_NetworkPrefab))
        {
            NetworkManager.Singleton.AddNetworkPrefab(m_NetworkPrefab);
            cNpcManager.Instance.AddToBeRemovedAtEndNetworkPrefab(m_NetworkPrefab);
        }
        if (IsHost)
        {
            GameObject go = Instantiate(m_NetworkPrefab, m_SpawnPoint.position, m_SpawnPoint.rotation);
            go.GetComponent<NetworkObject>().Spawn();

            var character = go.GetComponentInChildren<cCharacterNetworkController>();
            if (m_SetTeamId)
            {
                character.m_TeamId.Value = m_TeamId;
            }

            character.HealthBarState.Value = m_HealthBarState;
           
            cNpcManager.Instance.m_Npcs.Add(go);
            gameObject.SetActive(false);
            // go.GetComponentInChildren<cStateMachine>().m_enemies.AddRange(FindObjectsOfType<cPlayerStateMachineV2>().Select((v2 => v2.transform)).ToList());
        }
        
    }
}