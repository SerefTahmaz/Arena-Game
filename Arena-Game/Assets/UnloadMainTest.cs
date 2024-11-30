using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class UnloadMainTest : MonoBehaviour
{
    public void UnloadMain()
    {
        SceneManager.UnloadSceneAsync("Main");
    }
    
    public void ClearCharacterStates()
    {
    }
}



#if UNITY_EDITOR
[CustomEditor(typeof(UnloadMainTest))]
public class UnloadMainTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Click"))
        {
            (target as UnloadMainTest).ClearCharacterStates();
        }
    }
}
#endif
