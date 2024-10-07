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

public static class ItemExtensions
{
    public static T DuplicateUnique<T>(this T armorItemSo) where T : BaseItemSO
    {
        var uniqueDuplicateItem = GameObject.Instantiate(armorItemSo);
        uniqueDuplicateItem.RegenerateGuid();
        return uniqueDuplicateItem;
    }
}


public class TestRunTimeItem : MonoBehaviour
{
    [SerializeField] private CharacterSO m_Char;
    [SerializeField] private ArmorItemSO m_ArmorItemSo;

    public void CreateRuntimeItem()
    {
        var uniqueDuplicateItem = m_ArmorItemSo.DuplicateUnique();
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