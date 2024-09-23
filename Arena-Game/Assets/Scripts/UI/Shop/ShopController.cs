using System.Collections.Generic;
using DefaultNamespace;
using Gameplay;
using Gameplay.Item;
using UnityEngine;

namespace UI.Shop
{
    public class ShopController : MonoBehaviour,IMarketItemHandler
    {
        [SerializeField] private CharacterSO m_CharacterSo;
        [SerializeField] private MarketItemController m_MarketItemPrefab;
        [SerializeField] private Transform m_LayoutParent;
        [SerializeField] private MarketItemListSO m_MarketItemListSo;
        [SerializeField] private cMenuNode m_MenuNode;

        private List<MarketItemController> m_MarketItemControllers = new List<MarketItemController>();

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            m_MenuNode.OnActivateEvent.AddListener(Refresh);
            m_MenuNode.OnDeActivateEvent.AddListener(OnDeactivate);
        }

        private void OnDeactivate()
        {

            InventoryPreviewManager.Instance.ClearEquipment();
        }

        public void Refresh()
        {
            foreach (var VARIABLE in m_MarketItemControllers)
            {
                Destroy(VARIABLE.gameObject);
            }
            m_MarketItemControllers.Clear();
            m_CharacterSo.Load();
            foreach (var marketItemSo in m_MarketItemListSo.MarketItemSOs)
            {
                var ins = Instantiate(m_MarketItemPrefab,m_LayoutParent);
                ins.Init(marketItemSo,this);
                m_MarketItemControllers.Add(ins);
            }
        }

        public void HandlePurchase(MarketItemController marketItemController)
        {
            var isItemInInventory = m_CharacterSo.IsItemInInventory(marketItemController.MarketItemSo.RewardItemTemplate);
            if (!isItemInInventory)
            {
                m_CharacterSo.AddInventory(marketItemController.MarketItemSo.RewardItemTemplate);
                
                if(marketItemController.MarketItemSo.RewardItemTemplate is ArmorItemTemplate rewardArmorItem) 
                    m_CharacterSo.EquipItem(rewardArmorItem);
            }

            marketItemController.UnlockMarketItem();
        }

        public void HandlePreview(MarketItemController marketItemController)
        {
            if (!marketItemController.IsPreviewing)
            {
                marketItemController.SetPreviewState(true);
                InventoryPreviewManager.Instance.Equip(marketItemController.MarketItemSo.RewardItemTemplate as ArmorItemTemplate);
            }
            else
            {
                marketItemController.SetPreviewState(false);
                InventoryPreviewManager.Instance.Unequip(marketItemController.MarketItemSo.RewardItemTemplate as ArmorItemTemplate);
            }
        }
    }

    public interface IMarketItemHandler
    {
        void HandlePurchase(MarketItemController marketItemController);
        void HandlePreview(MarketItemController marketItemController);
    }
}