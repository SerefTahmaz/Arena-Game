using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level/Level")]
public class cLevelSO : ScriptableObject
{
#if UNITY_EDITOR
    public UnityEditor.SceneAsset m_SceneToLoad;

    private void OnValidate()
    {
        if (m_SceneToLoad != null)
        {
            m_SceneName = m_SceneToLoad.name;
        }
    }
#endif
    [SerializeField] private Sprite m_Icon;
    [SerializeField] private string m_NameText;
    private string m_SceneName;

    public Sprite Icon => m_Icon;

    public string NameText => m_NameText;

    public string SceneName => m_SceneName;
}
