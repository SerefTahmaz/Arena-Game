using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using RootMotion;
using Unity.Netcode;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class cNpcSpawner : cSingleton<cNpcSpawner>
{
    [SerializeField] private GameObject m_EnemyHuman;
    
    [SerializeField] private Transform m_TrollSpawnPoint;
    [SerializeField] private Transform m_DragonSpawnPoint;
    
    public GameObject EnemyHuman()
    {
        GameObject go = Instantiate(m_EnemyHuman, m_TrollSpawnPoint.position, Quaternion.identity);
        go.GetComponent<NetworkObject>().Spawn();
        return go;
    }
}



#if UNITY_EDITOR
[CustomEditor(typeof(cNpcSpawner))]
public class cNpcSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
#endif