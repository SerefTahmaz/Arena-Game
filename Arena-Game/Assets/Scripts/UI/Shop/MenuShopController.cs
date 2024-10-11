using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Gameplay;
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
            m_TargetCharacterSO.Load();
            var isItemInInventory = m_TargetCharacterSO.IsItemInInventory(marketItemController.RewardItem);
            if (!isItemInInventory)
            {
                m_TargetCharacterSO.AddInventory(marketItemController.RewardItem);
                
                if(marketItemController.RewardItem is ArmorItemSO rewardArmorItem) 
                    m_TargetCharacterSO.EquipItem(rewardArmorItem);
            }

            marketItemController.DecreaseAmount();
        }

        public override void HandlePreview(MarketItemController marketItemController)
        {
            base.HandlePreview(marketItemController);
            if (!marketItemController.IsPreviewing)
            {
                marketItemController.SetPreviewState(true);
                InventoryPreviewManager.Instance.Equip(marketItemController.RewardItem as ArmorItemSO);
            }
            else
            {
                marketItemController.SetPreviewState(false);
                InventoryPreviewManager.Instance.Unequip(marketItemController.RewardItem as ArmorItemSO);
            }
        }
    }
}