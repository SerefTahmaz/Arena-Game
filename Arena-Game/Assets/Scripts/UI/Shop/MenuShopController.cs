using System;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Gameplay;
using Gameplay.Item;
using UnityEngine;

namespace UI.Shop
{
    public class MenuShopController : ShopController
    {
        [SerializeField] private CharacterSO m_TargetCharacterSO;
        [SerializeField] private cMenuNode m_MenuNode;
        
        private void Awake()
        {
            Init();
        }
        
        public override void Init()
        {
            base.Init();
            m_MenuNode.OnActivateEvent.AddListener(Refresh);
            m_MenuNode.OnDeActivateEvent.AddListener(OnDeactivate);
        }
        
        private void UpdateItemsState()
        {
            foreach (var VARIABLE in m_MarketItemControllers)
            {
                if (VARIABLE.RewardItem is ArmorItemSO armorItemSo)
                {
                    VARIABLE.SetPreviewState(InventoryPreviewManager.Instance.IsItemEquiped(armorItemSo));
                }
            }
        }

        private void OnDeactivate()
        {
            InventoryPreviewManager.Instance.ClearEquipment();
        }

        public override async UniTask HandleBuy(MarketItemController marketItemController)
        {
            await base.HandleBuy(marketItemController);
            var popUp = GlobalFactory.PurchasePopUpFactory.Create();
            var result = await popUp.Init(null,
                GameplayStatics.GetPlayerCharacterSO(),marketItemController.RewardItem.ItemName, marketItemController.MarketItemSo.Price, false);

            if (result)
            {
                HandlePurchase(marketItemController);
            }
            else
            {
                Debug.Log("Purchase Canceled!");
            }
        }

        public override void HandlePurchase(MarketItemController marketItemController)
        {
            base.HandlePurchase(marketItemController);
            var isItemInInventory = m_TargetCharacterSO.GetCharacterSave().IsItemInInventory(marketItemController.RewardItem);
            if (!isItemInInventory)
            {
                m_TargetCharacterSO.GetCharacterSave().AddInventory(marketItemController.RewardItem);
                
                if(marketItemController.RewardItem is ArmorItemSO rewardArmorItem) 
                    m_TargetCharacterSO.GetCharacterSave().EquipItem(rewardArmorItem);
            }

            marketItemController.DecreaseAmount();
        }

        public override void HandlePreview(MarketItemController marketItemController)
        {
            base.HandlePreview(marketItemController);
            
            var armorItemSo = marketItemController.RewardItem as ArmorItemSO;
            if (!marketItemController.IsPreviewing)
            {
                InventoryPreviewManager.Instance.Equip(armorItemSo);
                marketItemController.SetPreviewState(true);
            }
            else
            {
                marketItemController.SetPreviewState(false);
                InventoryPreviewManager.Instance.Unequip(armorItemSo);
            }
            
            UpdateItemsState();
        }
    }
}