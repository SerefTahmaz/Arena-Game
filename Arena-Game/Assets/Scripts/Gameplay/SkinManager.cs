using System;
using System.Collections.Generic;
using Gameplay;
using Item;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SkinManager : MonoBehaviour
{
    [SerializeField] private CharacterSO m_CharacterSO;
    [SerializeField] private SkinnedMeshRenderer m_ReferenceSkinnedMesh;
    [SerializeField] private GameObject m_DefaultSet;

    private List<GameObject> SpawnedItems = new List<GameObject>();
    
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
        Equipment();

        m_CharacterSO.OnChanged += Equipment;
    }

    public void Equipment()
    {
        if(!Application.isPlaying) return;
        
        m_DefaultSet.SetActive(false);
        
        m_CharacterSO.Load();
        
        foreach (var VARIABLE in SpawnedItems)
        {
            Destroy(VARIABLE);
        }

        if (m_CharacterSO.HelmArmor != null)
        {
            var insArmor = Instantiate(m_CharacterSO.HelmArmor.ArmorPrefab);
            insArmor.Init(m_ReferenceSkinnedMesh);
            SpawnedItems.Add(insArmor.gameObject);
        }
        if (m_CharacterSO.ChestArmor != null)
        {
            var insArmor = Instantiate(m_CharacterSO.ChestArmor.ArmorPrefab);
            insArmor.Init(m_ReferenceSkinnedMesh);
            SpawnedItems.Add(insArmor.gameObject);
        }
        if (m_CharacterSO.GauntletsArmor != null)
        {
            var insArmor = Instantiate(m_CharacterSO.GauntletsArmor.ArmorPrefab);
            insArmor.Init(m_ReferenceSkinnedMesh);
            SpawnedItems.Add(insArmor.gameObject);
        }
        if (m_CharacterSO.LeggingArmor != null)
        {
            var insArmor = Instantiate(m_CharacterSO.LeggingArmor.ArmorPrefab);
            insArmor.Init(m_ReferenceSkinnedMesh);
            SpawnedItems.Add(insArmor.gameObject);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SkinManager))]
public class SkinManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var skinManager = target as SkinManager;
        
        if (GUILayout.Button("Click"))
        {
            skinManager.Equipment();
        }
    }
}
#endif
