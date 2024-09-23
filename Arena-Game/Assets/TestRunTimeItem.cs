using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using Gameplay.Item;
using Item.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using Guid = System.Guid;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class SaveableArmorItem
{
    public string TemplateArmorItemSOGuid;
    public int Level;
}

public class ArmorItemRuntime
{
    public ArmorItemTemplate m_TemplateArmorItemTemplate;
    public int m_Level;
}


public class TestRunTimeItem : MonoBehaviour
{
    [SerializeField] private CharacterSO m_Char;
    [FormerlySerializedAs("armorItemTemplateTemplateSo")] [FormerlySerializedAs("m_ArmorItemSO")] [SerializeField] private ArmorItemTemplate armorItemTemplateSo;

    public void CreateRuntimeItem()
    {
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(TestRunTimeItem))]
public class TemplateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Click"))
        {
            (target as TestRunTimeItem).CreateRuntimeItem();
        }
    }
}
#endif