using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Item;
using UnityEngine;
using Guid = Item.Scripts.Guid;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SkinManager : MonoBehaviour
{
    [SerializeField] private CharacterSO m_CharacterSO;
    public List<SkinPiece> m_Skins;
    
    [Serializable]
    public class SkinPiece
    {
        public BaseItemSO Key;
        public List<GameObject> Pieces;
        public bool EditorEnable = true;

        public void SetActive(bool value)
        {
            foreach (var VARIABLE in Pieces)
            {
                VARIABLE.SetActive(value);
            }
        }
    }

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        m_CharacterSO.Load();
        
        foreach (var VARIABLE in m_Skins)
        {
            VARIABLE.SetActive(false);
        }

        foreach (var VARIABLE in m_CharacterSO.EquipmentList)
        {
            var skin = m_Skins.FirstOrDefault((piece => piece.Key.Guid == VARIABLE.Guid));
            
            if (skin != null)
            {
                skin.SetActive(true);
            } 
        }
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
