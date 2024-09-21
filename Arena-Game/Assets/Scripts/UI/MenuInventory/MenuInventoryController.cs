using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using DefaultNamespace;
using Gameplay;
using Gameplay.Item;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MenuInventoryController : MonoBehaviour,IMenuInventoryItemHandler
{
    [SerializeField] private CharacterSO m_CharacterSo;
    [SerializeField] private MenuInventoryItemController m_MenuInventoryItemPrefab;
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
        m_CharacterSo.Load();
        foreach (var VARIABLE in m_CharacterSo.InventoryList)
        {
            var ins = Instantiate(m_MenuInventoryItemPrefab,m_LayoutParent);
            ins.Init(VARIABLE as ArmorItem, m_CharacterSo.IsItemEquiped(VARIABLE),this);
            m_InsInventoryItemControllers.Add(ins);
        }
    }

    public void HandleClick(MenuInventoryItemController menuInventoryItemController)
    {
        var isItemEquiped = m_CharacterSo.IsItemEquiped(menuInventoryItemController.m_Item);
        Debug.Log(isItemEquiped);
        if (isItemEquiped)
        {
            UnequipItem(menuInventoryItemController.m_Item);
        }
        else
        {
            EquipItem(menuInventoryItemController.m_Item);
        }

        UpdateItemsState();
    }

    private void UpdateItemsState()
    {
        foreach (var VARIABLE in m_InsInventoryItemControllers)
        {
            VARIABLE.SetEquipState(m_CharacterSo.IsItemEquiped(VARIABLE.m_Item));
        }
    }

    private void EquipItem(ArmorItem item)
    {
        m_CharacterSo.EquipItem(item);
    }

    private void UnequipItem(ArmorItem item)
    {
        m_CharacterSo.UnequipItem(item);
    }
}

public interface IMenuInventoryItemHandler
{
    void HandleClick(MenuInventoryItemController menuInventoryItemController);
}