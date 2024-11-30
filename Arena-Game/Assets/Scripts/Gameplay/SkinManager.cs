using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay;
using Gameplay.Item;
using Item;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SkinManager : MonoBehaviour
{
    [SerializeField] private CharacterSO m_CharacterSO;
    [SerializeField] private SkinnedMeshRenderer m_ReferenceSkinnedMesh;
    [SerializeField] private GameObject m_DefaultSet;
    [SerializeField] private GameObject m_HairGO;
    [SerializeField] private ArmorItemSO m_BasePant;

    private SkinArmor m_SpawnedHelm;
    private SkinArmor m_SpawnedChest;
    private SkinArmor m_SpawnedGauntlets;
    private SkinArmor m_SpawnedLegging;
    
    [Serializable]
    public class SkinArmor
    {
        public ArmorController m_ArmorController;
        public ArmorItemSO armorItemTemplate;
        public Action OnRemove { get; set; }
    }

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        Equipment();

        m_CharacterSO.GetCharacterSave().OnChanged += Equipment;
    } 
    
    private void OnDestroy()
    {
        if(m_CharacterSO.GetCharacterSave() != null) m_CharacterSO.GetCharacterSave().OnChanged -= Equipment;
    }

    public void Equipment()
    {
        if (!Application.isPlaying)
        {
            Debug.Log("Application");
            return;
        }
        
        m_DefaultSet.SetActive(false);
        
        ClearEquip(ArmorType.Helm);
        ClearEquip(ArmorType.Chest);
        ClearEquip(ArmorType.Gauntlets);
        ClearEquip(ArmorType.Legging);

        if (m_CharacterSO.GetCharacterSave().HelmArmor != null)
        {
            EquipItem(m_CharacterSO.GetCharacterSave().HelmArmor);
        }
        if (m_CharacterSO.GetCharacterSave().ChestArmor != null)
        {
            EquipItem(m_CharacterSO.GetCharacterSave().ChestArmor);
        }
        if (m_CharacterSO.GetCharacterSave().GauntletsArmor != null)
        {
            EquipItem(m_CharacterSO.GetCharacterSave().GauntletsArmor);
        }
        if (m_CharacterSO.GetCharacterSave().LeggingArmor != null)
        {
            EquipItem(m_CharacterSO.GetCharacterSave().LeggingArmor);
        }
        else
        {
            EquipItem(m_BasePant);
        }
    }

    public SkinArmor EquipItem(ArmorItemSO itemSO)
    {
        switch (itemSO.ArmorType)
        {
            case ArmorType.Helm:
                return EquipItem(itemSO, "_HelmMask", ref m_SpawnedHelm);
                break;
            case ArmorType.Chest:
                return EquipItem(itemSO, "_ChestMask", ref m_SpawnedChest);
                break;
            case ArmorType.Gauntlets:
                return EquipItem(itemSO, "_GauntletsMask", ref m_SpawnedGauntlets);
                break;
            case ArmorType.Legging:
                return EquipItem(itemSO, "_LeggingMask", ref m_SpawnedLegging);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private SkinArmor EquipItem(ArmorItemSO itemSO, string maskKey, ref SkinArmor spawnHolder)
    {
        ClearEquip(itemSO.ArmorType);
        //TODO: Handle hair state clearing
        
        var insArmor = Instantiate(itemSO.ItemTemplate.ArmorPrefab);
        insArmor.transform.SetParent(transform);
        insArmor.Init(m_ReferenceSkinnedMesh);
        spawnHolder = new SkinArmor() { armorItemTemplate = itemSO, m_ArmorController = insArmor };
        m_ReferenceSkinnedMesh.material.SetTexture(maskKey, itemSO.ItemTemplate.BodyMask);
        if(itemSO.ItemTemplate.HideHair) m_HairGO.SetActive(false);
        return spawnHolder;
    }

    public void DefaultEquip(ArmorType armorItemArmorType)
    {
        switch (armorItemArmorType)
        {
            case ArmorType.Helm:
                if (m_CharacterSO.GetCharacterSave().HelmArmor) EquipItem(m_CharacterSO.GetCharacterSave().HelmArmor); else ClearEquip(armorItemArmorType);
                break;
            case ArmorType.Chest:
                if (m_CharacterSO.GetCharacterSave().ChestArmor) EquipItem(m_CharacterSO.GetCharacterSave().ChestArmor); else ClearEquip(armorItemArmorType);
                break;
            case ArmorType.Gauntlets:
                if (m_CharacterSO.GetCharacterSave().GauntletsArmor) EquipItem(m_CharacterSO.GetCharacterSave().GauntletsArmor); else ClearEquip(armorItemArmorType);
                break;
            case ArmorType.Legging:
                if (m_CharacterSO.GetCharacterSave().LeggingArmor) EquipItem(m_CharacterSO.GetCharacterSave().LeggingArmor); else EquipItem(m_BasePant);
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
                    m_SpawnedHelm.OnRemove?.Invoke();
                    Destroy(m_SpawnedHelm.m_ArmorController.gameObject);
                    m_SpawnedHelm = null;
                }
                m_HairGO.SetActive(true);
                break;
            case ArmorType.Chest:
                m_ReferenceSkinnedMesh.material.SetTexture("_ChestMask", Texture2D.blackTexture);
                if (m_SpawnedChest != null) {
                    m_SpawnedChest.OnRemove?.Invoke();
                    Destroy(m_SpawnedChest.m_ArmorController.gameObject);
                    m_SpawnedChest = null;
                }
                break;
            case ArmorType.Gauntlets:
                m_ReferenceSkinnedMesh.material.SetTexture("_GauntletsMask", Texture2D.blackTexture);
                if (m_SpawnedGauntlets != null) {
                    m_SpawnedGauntlets.OnRemove?.Invoke();
                    Destroy(m_SpawnedGauntlets.m_ArmorController.gameObject);
                    m_SpawnedGauntlets = null;
                }
                break;
            case ArmorType.Legging:
                m_ReferenceSkinnedMesh.material.SetTexture("_LeggingMask", Texture2D.blackTexture);
                if (m_SpawnedLegging != null) {
                    m_SpawnedLegging.OnRemove?.Invoke();
                    Destroy(m_SpawnedLegging.m_ArmorController.gameObject);
                    m_SpawnedLegging = null;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(armorItemArmorType), armorItemArmorType, null);
        }
    }

    public void ClearAllEquipment()
    {
        ClearEquip(ArmorType.Helm);
        ClearEquip(ArmorType.Chest);
        ClearEquip(ArmorType.Gauntlets);
        ClearEquip(ArmorType.Legging);
    }
    
    public bool IsItemEquiped(ArmorItemSO armorItemSo)
    {
        var skinArmors = new List<SkinArmor>()
        {
            m_SpawnedHelm,
            m_SpawnedChest,
            m_SpawnedGauntlets,
            m_SpawnedLegging
        };
        return skinArmors.Any((armor => armor!=null && armor.armorItemTemplate == armorItemSo));
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
