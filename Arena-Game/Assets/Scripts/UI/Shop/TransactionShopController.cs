using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Gameplay;
using TMPro;
using UnityEngine;

namespace UI.Shop
{
    public class TransactionShopController : ShopController
    {
        [SerializeField] private TMP_Text m_ShopNameText;
        
        private CharacterSO m_SourceCharacter;
        private CharacterSO m_TargetCharacter;
        private bool m_IsPlayerSelling;

        public void Init(CharacterSO sourceCharacter, CharacterSO targetCharacter, bool isPlayerSelling)
        {
            base.Init();
            m_SourceCharacter = sourceCharacter;
            m_TargetCharacter = targetCharacter;
            m_IsPlayerSelling = isPlayerSelling;
            m_SourceCharacter.GetCharacterSave().OnChanged += Refresh;
            m_TargetCharacter.GetCharacterSave().OnChanged += Refresh;
            Refresh();

            m_ShopNameText.text = sourceCharacter.name;
        }

        public void CleanUp()
        {
            m_SourceCharacter.GetCharacterSave().OnChanged -= Refresh;
            m_TargetCharacter.GetCharacterSave().OnChanged -= Refresh;
        }

        private void OnDestroy()
        {
            if(m_SourceCharacter) m_SourceCharacter.GetCharacterSave().OnChanged -= Refresh;
            if(m_TargetCharacter) m_TargetCharacter.GetCharacterSave().OnChanged -= Refresh;
        }

        public override void Refresh()
        {
            m_MarketItemListSo = ScriptableObject.CreateInstance<MarketItemListSO>();
            m_MarketItemListSo.MarketItemSOs = new List<MarketItemSO>();
            foreach (var VARIABLE in m_SourceCharacter.GetCharacterSave().InventoryList)
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
            var isItemInInventory = m_TargetCharacter.GetCharacterSave().IsItemInInventory(marketItemController.RewardItem);
            if (!isItemInInventory)
            {
                m_TargetCharacter.GetCharacterSave().AddInventory(marketItemController.RewardItem);
                m_TargetCharacter.GetCharacterSave().Save();
            }
            m_SourceCharacter.GetCharacterSave().RemoveInventory(marketItemController.MarketItemSo.RewardItemTemplate);
            m_SourceCharacter.GetCharacterSave().Save();
            marketItemController.DecreaseAmount();
            Refresh();
        }
    }
}