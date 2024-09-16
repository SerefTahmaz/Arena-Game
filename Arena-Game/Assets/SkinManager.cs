using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SkinManager : MonoBehaviour
{
    public List<SkinPiece> m_Skins;
    
    [Serializable]
    public class SkinPiece
    {
        public string Key;
        public List<GameObject> Pieces;
        public bool EditorEnable = true;
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        foreach (var VARIABLE in m_Skins)
        {
            foreach (var p in VARIABLE.Pieces)
            {
                if (p.activeSelf != VARIABLE.EditorEnable)
                {
                    p.SetActive(VARIABLE.EditorEnable);
                    EditorUtility.SetDirty(p);
                }
            }
        }
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(SkinManager))]
public class SkinManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // var skinManager = target as SkinManager;
        //
        // var fiels = serializedObject.get
        //
        // if (GUILayout.Button("Click"))
        // {
        //     ().MethodName();
        // }
    }
}
#endif
