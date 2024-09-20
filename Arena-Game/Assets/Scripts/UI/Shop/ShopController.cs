using DefaultNamespace;
using Gameplay;
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
            m_CharacterSo.Load();
            foreach (var marketItemSo in m_MarketItemListSo.MarketItemSOs)
            {
                var ins = Instantiate(m_MarketItemPrefab,m_LayoutParent);
                ins.Init(marketItemSo,this);
            }
        }

        public void HandlePurchase(MarketItemController marketItemController)
        {
            var isItemInInventory = m_CharacterSo.IsItemInInventory(marketItemController.MarketItemSo.RewardItem);
            if (!isItemInInventory)
            {
                m_CharacterSo.AddInventory(marketItemController.MarketItemSo.RewardItem);
            }

            marketItemController.UnlockMarketItem();
        }
    }

    public interface IMarketItemHandler
    {
        void HandlePurchase(MarketItemController marketItemController);
    }
}