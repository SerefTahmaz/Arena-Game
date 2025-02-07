using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using DefaultNamespace;
using Gameplay.Item;
using UnityEngine;

public class InventoryPreviewManager : cSingleton<InventoryPreviewManager>
{
    [SerializeField] private GameObject m_Pivot;
    [SerializeField] private SkinManager m_SkinManager;
    [SerializeField] private Transform m_FreeroamLevelPivot;
    [SerializeField] private Transform m_ArenaLevelPivot;
    
    public Action<ArmorType> OnArmorEquip { get; set; }

    private void Awake()
    {
        MapManager.Instance.OnMapLoaded += HandleMapLoaded;
    }

    private void HandleMapLoaded(int levelIndex)
    {
        if (levelIndex == -1)
        {
            transform.SetParentResetTransform(m_FreeroamLevelPivot);
        }
        else
        {
            transform.SetParentResetTransform(m_ArenaLevelPivot);
        }
    }

    public SkinManager.SkinArmor Equip(ArmorItemSO armorItemTemplate)
    {
        OnArmorEquip?.Invoke(armorItemTemplate.ArmorType);
        return m_SkinManager.EquipItem(armorItemTemplate);
    }

    public void Unequip(ArmorItemSO armorItemTemplate)
    {
        m_SkinManager.DefaultEquip(armorItemTemplate.ArmorType);
    }

    public void ClearEquipment()
    {
        m_SkinManager.Equipment();
    }

    public void SetState(bool state)
    {
        m_Pivot.SetActive(state);
    }
    
    public bool IsItemEquiped(ArmorItemSO armorItemSo)
    {
        return m_SkinManager.IsItemEquiped(armorItemSo);
    }
}
