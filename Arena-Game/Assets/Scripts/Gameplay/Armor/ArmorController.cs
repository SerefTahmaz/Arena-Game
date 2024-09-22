using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class ArmorController : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer m_Reference;
    [SerializeField] private List<SkinnedMeshRenderer> m_TargetRefs;

    public void Init(SkinnedMeshRenderer referenceMesh)
    {
        m_Reference = referenceMesh;
        RemapSkinnedMeshes();
    }
    
    public void RemapSkinnedMeshes()
    {
        foreach (var skinnedMeshRenderer in m_TargetRefs)
        {
            skinnedMeshRenderer.bones = m_Reference.bones;
            skinnedMeshRenderer.rootBone = m_Reference.rootBone;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ArmorController))]
public class ArmorControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("RemapSkinnedMeshes"))
        {
            (target as ArmorController).RemapSkinnedMeshes();
        }
    }
}
#endif
