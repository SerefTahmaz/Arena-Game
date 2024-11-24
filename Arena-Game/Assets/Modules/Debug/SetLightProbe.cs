using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SetLightProbe : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private Transform m_Anchor;

    public void SetAnchor()
    {
        var meshes = GetComponentsInChildren<Renderer>();
        foreach (var VARIABLE in meshes)
        {
            VARIABLE.probeAnchor = m_Anchor;
            EditorUtility.SetDirty(VARIABLE);
        }
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(SetLightProbe))]
public class SetLightProbeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Click"))
        {
            (target as SetLightProbe).SetAnchor();
        }
    }
}
#endif