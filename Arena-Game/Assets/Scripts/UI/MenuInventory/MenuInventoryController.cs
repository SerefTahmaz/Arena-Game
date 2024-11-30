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

public class MenuInventoryController : ItemViewer,IMenuInventoryItemHandler
{
    [SerializeField] private CharacterSO m_CharacterSo;
    [SerializeField] private cMenuNode m_MenuNode;

    private void Awake()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
        m_MenuNode.OnActivateEvent.AddListener((() =>
        {
            Refresh(m_CharacterSo.GetCharacterSave().InventoryList);
        }));
    }

    protected override void Refresh(List<BaseItemSO> itemSos)
    {
        base.Refresh(itemSos);
        UpdateItemsState();
    }

    public override void HandleClick(MenuInventoryItemController menuInventoryItemController)
    {
        base.HandleClick(menuInventoryItemController);
        if (menuInventoryItemController is ArmorMenuInventoryItemController armorInventoryItem)
        {
            var isItemEquiped = m_CharacterSo.GetCharacterSave().IsItemEquiped(armorInventoryItem.itemTemplate);
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
                armorMenuInventoryItemController.SetEquipState(m_CharacterSo.GetCharacterSave().IsItemEquiped(armorMenuInventoryItemController.itemTemplate));
            }
        }
    }
 
    private void EquipItem(ArmorItemSO itemTemplate)
    {
        m_CharacterSo.GetCharacterSave().EquipItem(itemTemplate);
    }

    private void UnequipItem(ArmorItemSO itemTemplate)
    {
        m_CharacterSo.GetCharacterSave().UnequipItem(itemTemplate);
    }
}