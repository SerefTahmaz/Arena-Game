using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RuntimeShadowChanger : MonoBehaviour
{
    [SerializeField] private UniversalRenderPipelineAsset m_DialogURPSettings;


    public void Change()
    {
        QualitySettings.renderPipeline = m_DialogURPSettings;
    }
}



#if UNITY_EDITOR
[CustomEditor(typeof(RuntimeShadowChanger))]
public class RuntimeShadowChangerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Click"))
        {
            (target as RuntimeShadowChanger).Change();
        }
    }
}
#endif