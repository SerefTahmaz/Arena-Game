using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DebugShopHelper : MonoBehaviour
{
    [SerializeField] private float m_StartValue;
    [SerializeField] private float m_EndValue;

    [SerializeField] private List<float> m_Items;
    [SerializeField] private List<float> m_SideItems;

    [SerializeField] private int m_ItemCount;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Calculate()
    {
        m_Items.Clear();
        m_SideItems.Clear();
        for (int i = 0; i < m_ItemCount; i++)
        {
            var nextItem = (float)i;
            nextItem=nextItem.Remap(0, m_ItemCount - 1, m_StartValue, m_EndValue);
            m_Items.Add((int)nextItem);
            m_SideItems.Add((int)(nextItem*(1/1.6f)));
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DebugShopHelper))]
public class DebugShopHelperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Click"))
        {
            (target as DebugShopHelper).Calculate();
        }
    }
}
#endif
