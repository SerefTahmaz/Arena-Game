using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace DefaultNamespace
{
    public class SetQualitySettingsRuntime : MonoBehaviour
    {
        [SerializeField] private UniversalRenderPipelineAsset m_URPSettings;

        public void ApplyQualitySetting()
        {
            QualitySettings.renderPipeline = m_URPSettings;
        }
        
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(SetQualitySettingsRuntime))]
    public class TemplateEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("ApplyQualitySetting"))
            {
                (target as SetQualitySettingsRuntime).ApplyQualitySetting();
            }
        }
    }
#endif
}


