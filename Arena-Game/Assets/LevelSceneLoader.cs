using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelSceneLoader : MonoBehaviour
{
    [SerializeField] private SceneAsset m_SceneAsset;
    

    public async UniTask LoadLevel()
    {
        await SceneManager.LoadSceneAsync(m_SceneAsset.name, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(m_SceneAsset.name));
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(LevelSceneLoader))]
public class LevelSceneLoaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Click"))
        {
            (target as LevelSceneLoader).LoadLevel();
        }
    }
}
#endif