using System;
using System.Collections.Generic;
using Gameplay;
using Item;
using UnityEngine;

namespace ArenaGame.UI.MenuInventory
{

    public class ItemViewer : MonoBehaviour,IMenuInventoryItemHandler
    {
        [SerializeField] private ArmorMenuInventoryItemController m_MenuInventoryItemPrefab;
        [SerializeField] private ConsumableInventoryItemController m_ConsumableInventoryItemPrefab;
        [SerializeField] private Transform m_LayoutParent;
        protected List<MenuInventoryItemController> m_InsInventoryItemControllers = new List<MenuInventoryItemController>();


        protected virtual void Init()
        {
        }

        protected virtual void Refresh(List<BaseItemSO> itemSos)
        {
            foreach (var VARIABLE in m_InsInventoryItemControllers)
            {
                Destroy(VARIABLE.gameObject);
            }
            m_InsInventoryItemControllers.Clear();
            foreach (var VARIABLE in itemSos)
            {
                Debug.Log(VARIABLE.ItemType);
                switch (VARIABLE.ItemType)
                {
                    case ItemType.Weapon:
                        break;
                    case ItemType.Armor:
                        var insArmorItemUI = Instantiate(m_MenuInventoryItemPrefab,m_LayoutParent);
                        insArmorItemUI.Init(VARIABLE as ArmorItemSO,this);
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

        public virtual void HandleClick(MenuInventoryItemController menuInventoryItemController)
        {
            
        }
        
    }

    public interface IMenuInventoryItemHandler
    {
        void HandleClick(MenuInventoryItemController menuInventoryItemController);
    }
}