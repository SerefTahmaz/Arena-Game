using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using Gameplay.Item;
using UnityEngine;

public class InventoryPreviewManager : cSingleton<InventoryPreviewManager>
{
    [SerializeField] private SkinManager m_SkinManager;

    public void Equip(ArmorItemSO armorItemTemplate)
    {
        m_SkinManager.EquipItem(armorItemTemplate);
    }

    public void Unequip(ArmorItemSO armorItemTemplate)
    {
        m_SkinManager.DefaultEquip(armorItemTemplate.ArmorType);
    }

    public void ClearEquipment()
    {
        m_SkinManager.Equipment();
    }
}
