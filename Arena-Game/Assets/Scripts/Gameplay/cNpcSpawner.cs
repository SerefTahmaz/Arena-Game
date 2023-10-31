using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class cNpcSpawner : MonoBehaviour
{
    [SerializeField] private GameObject m_Dragon;
    [SerializeField] private Transform m_SpawnPoint;

    public void Spawn()
    {
        GameObject go = Instantiate(m_Dragon, m_SpawnPoint.position, Quaternion.identity);
        go.GetComponent<NetworkObject>().Spawn();
    }

    private void Update()
    {
        if (NetworkManager.Singleton.IsHost&& Input.GetKeyDown(KeyCode.T))
        {
            Spawn();
            Destroy(gameObject);
        }
    }
}



#if UNITY_EDITOR
[CustomEditor(typeof(cNpcSpawner))]
public class cNpcSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Click"))
        {
            (target as cNpcSpawner).Spawn();
        }
    }
}
#endif