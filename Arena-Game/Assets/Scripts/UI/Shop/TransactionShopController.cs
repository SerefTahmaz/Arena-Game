using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Gameplay;
using UnityEngine;

namespace UI.Shop
{
    public class TransactionShopController : ShopController
    {
        private CharacterSO m_SourceCharacter;
        private CharacterSO m_TargetCharacter;
        private bool m_IsPlayerSelling;

        public void Init(CharacterSO sourceCharacter, CharacterSO targetCharacter, bool isPlayerSelling)
        {
            base.Init();
            m_SourceCharacter = sourceCharacter;
            m_TargetCharacter = targetCharacter;
            m_IsPlayerSelling = isPlayerSelling;
            m_SourceCharacter.OnChanged += Refresh;
            m_TargetCharacter.OnChanged += Refresh;
            Refresh();
        }

        public void CleanUp()
        {
            m_SourceCharacter.OnChanged -= Refresh;
            m_TargetCharacter.OnChanged -= Refresh;
        }

        private void OnDestroy()
        {
            if(m_SourceCharacter) m_SourceCharacter.OnChanged -= Refresh;
            if(m_TargetCharacter) m_TargetCharacter.OnChanged -= Refresh;
        }

        public override void Refresh()
        {
            m_MarketItemListSo = ScriptableObject.CreateInstance<MarketItemListSO>();
            m_MarketItemListSo.MarketItemSOs = new List<MarketItemSO>();
            m_SourceCharacter.Load();
            foreach (var VARIABLE in m_SourceCharacter.InventoryList)
            {
                if (VARIABLE is ISellableItem sellableItem)
                {
                    var insMarketItem = ScriptableObject.CreateInstance<MarketItemSO>();
                    insMarketItem.RewardItemTemplate = VARIABLE;
                    insMarketItem.Price = sellableItem.Price;
                    m_MarketItemListSo.MarketItemSOs.Add(insMarketItem);
                }
            }
            base.Refresh();
        }

        public override async UniTask HandleBuy(MarketItemController marketItemController)
        {
            await base.HandleBuy(marketItemController);
            var popUp = GlobalFactory.PurchasePopUpFactory.Create();
            var result = await popUp.Init(m_SourceCharacter,m_TargetCharacter,marketItemController.RewardItem.ItemName, 
                marketItemController.MarketItemSo.Price, m_IsPlayerSelling);

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
            m_TargetCharacter.Load();
            var isItemInInventory = m_TargetCharacter.IsItemInInventory(marketItemController.RewardItem);
            if (!isItemInInventory)
            {
                m_TargetCharacter.AddInventory(marketItemController.RewardItem);
                m_TargetCharacter.Save();
            }
            m_SourceCharacter.Load();
            m_SourceCharacter.RemoveInventory(marketItemController.MarketItemSo.RewardItemTemplate);
            m_SourceCharacter.Save();
            marketItemController.DecreaseAmount();
            Refresh();
        }
    }
}