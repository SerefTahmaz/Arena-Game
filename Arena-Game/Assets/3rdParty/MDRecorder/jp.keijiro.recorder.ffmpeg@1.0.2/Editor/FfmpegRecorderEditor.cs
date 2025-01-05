using UnityEngine;
using UnityEditor;
using UnityEditor.Recorder;
using System.Linq;

namespace FFmpegOut.Recorder
{
    [CustomEditor(typeof(FfmpegRecorderSettings))]
    class FfmpegRecorderEditor : RecorderEditor
    {
        SerializedProperty _preset;
        SerializedProperty _flipImage;

        static class Styles
        {
            public static readonly GUIContent flipImage = new GUIContent("Flip Image");
        }

        GUIContent[] _presetLabels;
        int[] _presetOptions;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (target == null) return;

            _preset = serializedObject.FindProperty("preset");
            _flipImage = serializedObject.FindProperty("flipImage");

            // Preset labels
            var presets = FFmpegPreset.GetValues(typeof(FFmpegPreset));
            _presetLabels = presets.Cast<FFmpegPreset>().
                Select(p => new GUIContent(p.GetDisplayName())).ToArray();
            _presetOptions = presets.Cast<int>().ToArray();
        }

        protected override void FileTypeAndFormatGUI()
        {
            EditorGUILayout.IntPopup(_preset, _presetLabels, _presetOptions);

            var wide = EditorGUIUtility.labelWidth > 140;

            if (wide)
                EditorGUILayout.PropertyField(_flipImage);
            else
                EditorGUILayout.PropertyField(_flipImage, Styles.flipImage);
        }
    }
}
