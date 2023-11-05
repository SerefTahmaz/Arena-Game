using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class cNpcSpawnerProxy : NetworkBehaviour
{
    [SerializeField] private GameObject m_NetworkPrefab;
    [SerializeField] private Transform m_SpawnPoint;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        SpawnNpc();
    }

    public void SpawnNpc()
    {
        GameObject go = Instantiate(m_NetworkPrefab, m_SpawnPoint.position, m_SpawnPoint.rotation);
        go.GetComponent<NetworkObject>().Spawn();
        gameObject.SetActive(false);
    }
}