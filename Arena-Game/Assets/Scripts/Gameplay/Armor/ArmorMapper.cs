using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ArmorMapper : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer m_Reference;
    [SerializeField] private List<SkinnedMeshRenderer> m_TargetRefs;
    
    public void RemapSkinnedMeshes()
    {
        foreach (var VARIABLE in m_TargetRefs)
        {
            VARIABLE.bones = m_Reference.bones;
            VARIABLE.rootBone = m_Reference.rootBone;
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                EditorUtility.SetDirty(VARIABLE);
            }
#endif
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ArmorMapper))]
public class ArmorMapperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("RemapSkinnedMeshes"))
        {
            (target as ArmorMapper).RemapSkinnedMeshes();
        }
    }
}
#endif