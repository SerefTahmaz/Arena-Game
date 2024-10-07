using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.UI.MenuInventory;
using ArenaGame.Utils;
using DefaultNamespace;
using Gameplay;
using Gameplay.Item;
using Item;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MenuInventoryController : MonoBehaviour,IMenuInventoryItemHandler
{
    [SerializeField] private CharacterSO m_CharacterSo;
    [SerializeField] private ArmorMenuInventoryItemController m_MenuInventoryItemPrefab;
    [SerializeField] private ConsumableInventoryItemController m_ConsumableInventoryItemPrefab;
    [SerializeField] private Transform m_LayoutParent;
    [SerializeField] private cMenuNode m_MenuNode;

    private List<MenuInventoryItemController> m_InsInventoryItemControllers = new List<MenuInventoryItemController>();

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        m_MenuNode.OnActivateEvent.AddListener(Refresh);
    }

    public void Refresh()
    {
        foreach (var VARIABLE in m_InsInventoryItemControllers)
        {
            Destroy(VARIABLE.gameObject);
        }
        m_InsInventoryItemControllers.Clear();
        m_CharacterSo.Load();
        foreach (var VARIABLE in m_CharacterSo.InventoryList)
        {
            Debug.Log(VARIABLE.ItemType);
            switch (VARIABLE.ItemType)
            {
                case ItemType.Weapon:
                    break;
                case ItemType.Armor:
                    var insArmorItemUI = Instantiate(m_MenuInventoryItemPrefab,m_LayoutParent);
                    insArmorItemUI.Init(VARIABLE as ArmorItemSO, m_CharacterSo.IsItemEquiped(VARIABLE as ArmorItemSO),this);
                    m_InsInventoryItemControllers.Add(insArmorItemUI);
                    break;
                case ItemType.Consumable:
                    var insConsumableItemUI = Instantiate(m_ConsumableInventoryItemPrefab,m_LayoutParent);
                    insConsumableItemUI.Init(VARIABLE,this);
                    m_InsInventoryItemControllers.Add(insConsumableItemUI);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public void HandleClick(MenuInventoryItemController menuInventoryItemController)
    {
        if (menuInventoryItemController is ArmorMenuInventoryItemController armorInventoryItem)
        {
            var isItemEquiped = m_CharacterSo.IsItemEquiped(armorInventoryItem.itemTemplate);
            Debug.Log(isItemEquiped);
            if (isItemEquiped)
            {
                UnequipItem(armorInventoryItem.itemTemplate);
            }
            else
            {
                EquipItem(armorInventoryItem.itemTemplate);
            }
        }

        if (menuInventoryItemController is ConsumableInventoryItemController consumableInventoryItem)
        {
            
        }

        UpdateItemsState();
    }

    private void UpdateItemsState()
    {
        foreach (var VARIABLE in m_InsInventoryItemControllers)
        {
            if (VARIABLE is ArmorMenuInventoryItemController armorMenuInventoryItemController)
            {
                armorMenuInventoryItemController.SetEquipState(m_CharacterSo.IsItemEquiped(armorMenuInventoryItemController.itemTemplate));
            }
        }
    }
 
    private void EquipItem(ArmorItemSO itemTemplate)
    {
        m_CharacterSo.EquipItem(itemTemplate);
    }

    private void UnequipItem(ArmorItemSO itemTemplate)
    {
        m_CharacterSo.UnequipItem(itemTemplate);
    }
}

public interface IMenuInventoryItemHandler
{
    void HandleClick(MenuInventoryItemController menuInventoryItemController);
}