using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using Gameplay.Item;
using UnityEngine;

public class InventoryPreviewManager : cSingleton<InventoryPreviewManager>
{
    [SerializeField] private SkinManager m_SkinManager;

    public void Equip(ArmorItem armorItem)
    {
        m_SkinManager.EquipItem(armorItem);
    }

    public void Unequip(ArmorItem armorItem)
    {
        m_SkinManager.DefaultEquip(armorItem.ArmorType);
    }

    public void ClearEquipment()
    {
        m_SkinManager.Equipment();
    }
}
