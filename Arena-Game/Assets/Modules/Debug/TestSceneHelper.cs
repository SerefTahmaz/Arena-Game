using System.Collections;
using System.Collections.Generic;
using Gameplay.Character;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TestSceneHelper : MonoBehaviour
{
#if UNITY_EDITOR
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
#endif
}
