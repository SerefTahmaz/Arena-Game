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

    private SkinArmor m_SpawnedHelm;
    private SkinArmor m_SpawnedChest;
    private SkinArmor m_SpawnedGauntlets;
    private SkinArmor m_SpawnedLegging;
    
    [Serializable]
    public class SkinArmor
    {
        public ArmorController m_ArmorController;
        public ArmorItem ArmorItem;
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
        
        ClearEquip(ArmorType.Helm);
        ClearEquip(ArmorType.Chest);
        ClearEquip(ArmorType.Gauntlets);
        ClearEquip(ArmorType.Legging);

        if (m_CharacterSO.HelmArmor != null)
        {
            EquipItem(m_CharacterSO.HelmArmor);
        }
        if (m_CharacterSO.ChestArmor != null)
        {
            EquipItem(m_CharacterSO.ChestArmor);
        }
        if (m_CharacterSO.GauntletsArmor != null)
        {
            EquipItem(m_CharacterSO.GauntletsArmor);
        }
        if (m_CharacterSO.LeggingArmor != null)
        {
            EquipItem(m_CharacterSO.LeggingArmor);
        }
    }

    public void EquipItem(ArmorItem item)
    {
        switch (item.ArmorType)
        {
            case ArmorType.Helm:
                EquipItem(item, "_HelmMask", ref m_SpawnedHelm);
                break;
            case ArmorType.Chest:
                EquipItem(item, "_ChestMask", ref m_SpawnedChest);
                break;
            case ArmorType.Gauntlets:
                EquipItem(item, "_GauntletsMask", ref m_SpawnedGauntlets);
                break;
            case ArmorType.Legging:
                EquipItem(item, "_LeggingMask", ref m_SpawnedLegging);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void EquipItem(ArmorItem item, string maskKey, ref SkinArmor spawnHolder)
    {
        ClearEquip(item.ArmorType);
        //TODO: Handle hair state clearing
        
        var insArmor = Instantiate(item.ArmorPrefab);
        insArmor.transform.SetParent(transform);
        insArmor.Init(m_ReferenceSkinnedMesh);
        spawnHolder = new SkinArmor() { ArmorItem = item, m_ArmorController = insArmor };
        m_ReferenceSkinnedMesh.material.SetTexture(maskKey, item.BodyMask);
        if(item.HideHair) m_HairGO.SetActive(false);
    }

    public void DefaultEquip(ArmorType armorItemArmorType)
    {
        switch (armorItemArmorType)
        {
            case ArmorType.Helm:
                if (m_CharacterSO.HelmArmor) EquipItem(m_CharacterSO.HelmArmor); else ClearEquip(armorItemArmorType);
                break;
            case ArmorType.Chest:
                if (m_CharacterSO.ChestArmor) EquipItem(m_CharacterSO.ChestArmor); else ClearEquip(armorItemArmorType);
                break;
            case ArmorType.Gauntlets:
                if (m_CharacterSO.GauntletsArmor) EquipItem(m_CharacterSO.GauntletsArmor); else ClearEquip(armorItemArmorType);
                break;
            case ArmorType.Legging:
                if (m_CharacterSO.LeggingArmor) EquipItem(m_CharacterSO.LeggingArmor); else ClearEquip(armorItemArmorType);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(armorItemArmorType), armorItemArmorType, null);
        }
    }

    public void ClearEquip(ArmorType armorItemArmorType)
    {
        switch (armorItemArmorType)
        {
            case ArmorType.Helm:
                m_ReferenceSkinnedMesh.material.SetTexture("_HelmMask", Texture2D.blackTexture);
                if (m_SpawnedHelm != null) {
                    Destroy(m_SpawnedHelm.m_ArmorController.gameObject);
                    m_SpawnedHelm = null;
                }
                m_HairGO.SetActive(true);
                break;
            case ArmorType.Chest:
                m_ReferenceSkinnedMesh.material.SetTexture("_ChestMask", Texture2D.blackTexture);
                if (m_SpawnedChest != null) {
                    Destroy(m_SpawnedChest.m_ArmorController.gameObject);
                    m_SpawnedChest = null;
                }
                break;
            case ArmorType.Gauntlets:
                m_ReferenceSkinnedMesh.material.SetTexture("_GauntletsMask", Texture2D.blackTexture);
                if (m_SpawnedGauntlets != null) {
                    Destroy(m_SpawnedGauntlets.m_ArmorController.gameObject);
                    m_SpawnedGauntlets = null;
                }
                break;
            case ArmorType.Legging:
                m_ReferenceSkinnedMesh.material.SetTexture("_LeggingMask", Texture2D.blackTexture);
                if (m_SpawnedLegging != null) {
                    Destroy(m_SpawnedLegging.m_ArmorController.gameObject);
                    m_SpawnedLegging = null;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(armorItemArmorType), armorItemArmorType, null);
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
