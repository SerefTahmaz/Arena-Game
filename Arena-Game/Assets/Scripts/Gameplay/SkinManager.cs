using System;
using System.Collections.Generic;
using Gameplay;
using Gameplay.Item;
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
    [SerializeField] private GameObject m_HairGO;

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
        
        m_ReferenceSkinnedMesh.material.SetTexture("_HelmMask", Texture2D.blackTexture);
        m_ReferenceSkinnedMesh.material.SetTexture("_ChestMask", Texture2D.blackTexture);
        m_ReferenceSkinnedMesh.material.SetTexture("_GauntletsMask", Texture2D.blackTexture);
        m_ReferenceSkinnedMesh.material.SetTexture("_LeggingMask", Texture2D.blackTexture);
        m_HairGO.SetActive(true);

        if (m_CharacterSO.HelmArmor != null)
        {
            EquipItem(m_CharacterSO.HelmArmor, "_HelmMask");
        }
        if (m_CharacterSO.ChestArmor != null)
        {
            EquipItem(m_CharacterSO.ChestArmor, "_ChestMask");
        }
        if (m_CharacterSO.GauntletsArmor != null)
        {
            EquipItem(m_CharacterSO.GauntletsArmor, "_GauntletsMask");
        }
        if (m_CharacterSO.LeggingArmor != null)
        {
            EquipItem(m_CharacterSO.LeggingArmor, "_LeggingMask");
        }
    }

    private void EquipItem(ArmorItem item, string maskKey)
    {
        var insArmor = Instantiate(item.ArmorPrefab);
        insArmor.Init(m_ReferenceSkinnedMesh);
        SpawnedItems.Add(insArmor.gameObject);
        m_ReferenceSkinnedMesh.material.SetTexture(maskKey, item.BodyMask);
        if(item.HideHair) m_HairGO.SetActive(false);
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
