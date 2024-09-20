using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using DefaultNamespace;
using Gameplay;
using UnityEngine;

public class MenuInventoryController : MonoBehaviour,IMenuInventoryItemHandler
{
    [SerializeField] private CharacterSO m_CharacterSo;
    [SerializeField] private MenuInventoryItemController m_MenuInventoryItemPrefab;
    [SerializeField] private Transform m_LayoutParent;
    [SerializeField] private cMenuNode m_MenuNode;

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
        foreach (var VARIABLE in m_LayoutParent.gameObject.GetChilds())
        {
            Destroy(VARIABLE.gameObject);
        }
        m_CharacterSo.Load();
        foreach (var VARIABLE in m_CharacterSo.InventoryList)
        {
            var ins = Instantiate(m_MenuInventoryItemPrefab,m_LayoutParent);
            ins.Init(VARIABLE as ArmorItem, m_CharacterSo.IsItemEquiped(VARIABLE),this);
        }
    }

    public void HandleClick(MenuInventoryItemController menuInventoryItemController)
    {
        var isItemEquiped = m_CharacterSo.IsItemEquiped(menuInventoryItemController.m_Item);
        if (isItemEquiped)
        {
            UnequipItem(menuInventoryItemController.m_Item);
        }
        else
        {
            EquipItem(menuInventoryItemController.m_Item);
        }
        menuInventoryItemController.SetEquipState(!isItemEquiped);
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