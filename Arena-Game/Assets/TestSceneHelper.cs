using System.Collections;
using System.Collections.Generic;
using Gameplay.Character;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneHelper : MonoBehaviour
{
    [SerializeField] private bool m_SpawnPlayerOnStart;
    [SerializeField] private SceneAsset m_DebugLevel;
    
      
    // Start is called before the first frame update
    void Start()
    {
        if (m_SpawnPlayerOnStart)
        {
            SceneManager.LoadScene(m_DebugLevel.name,LoadSceneMode.Additive);
        }
    }
}
