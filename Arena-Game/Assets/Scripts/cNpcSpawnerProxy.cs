using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class cNpcSpawnerProxy : NetworkBehaviour
{
    [SerializeField] private GameObject m_NetworkPrefab;
    [SerializeField] private Transform m_SpawnPoint;
    [SerializeField] private cHealthManager.eHealthBarState m_HealthBarState;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        SpawnNpc();
    }

    public void SpawnNpc()
    {
        if (IsHost)
        {
            GameObject go = Instantiate(m_NetworkPrefab, m_SpawnPoint.position, m_SpawnPoint.rotation);
            go.GetComponent<NetworkObject>().Spawn();
            foreach (var VARIABLE in  go.GetComponentsInChildren<cHealthManager>())
            {
                VARIABLE.HealthBarState = m_HealthBarState;
            }
           
            cNpcManager.Instance.m_Npcs.Add(go);
        }
        gameObject.SetActive(false);
    }
}