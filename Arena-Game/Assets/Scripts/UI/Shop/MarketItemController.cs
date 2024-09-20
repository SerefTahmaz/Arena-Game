using Cysharp.Threading.Tasks;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shop
{
    public class MarketItemController : MonoBehaviour
    { 
        [SerializeField] private Image m_Image;
        [SerializeField] private TMP_Text m_Price;
        [SerializeField] private GameObject m_UnlockedLayer;
    
        private MarketItemSO m_MarketItemSo;
        private IMarketItemHandler m_MarketItemHandler;

        private bool IsUnlocked => MarketItemSo.IsUnlocked();

        public MarketItemSO MarketItemSo => m_MarketItemSo;

        public void Init(MarketItemSO marketItemSo, IMarketItemHandler marketItemHandler)
        {
            m_MarketItemSo = marketItemSo;
            m_MarketItemHandler = marketItemHandler;
        
            m_Image.sprite = MarketItemSo.RewardItem.ItemSprite;

            UpdateUI();
        }

        public void UpdateUI()
        {
            if (IsUnlocked)
            {
                m_Price.text = "";
            }
            else
            {
                m_Price.text = MarketItemSo.Price.ToString();
            }
            
            m_UnlockedLayer.SetActive(IsUnlocked);
        }
    
        public void Buy()
        {
            BuyAsync();
        }

        private async UniTask BuyAsync()
        {
            if(IsUnlocked) return;
            
            var popUp = GameFactorySingleton.Instance.PurchasePopUpFactory.Create();
            var result = await popUp.Init(MarketItemSo.RewardItem.ItemName, MarketItemSo.Price.ToString());

            if (result)
            {
                HandlePurchase();
            }
            else
            {
                Debug.Log("Purchase Canceled!");
            }
        }

        private void HandlePurchase()
        {
            m_MarketItemHandler.HandlePurchase(this);
        }

        public void UnlockMarketItem()
        {
            MarketItemSo.UnlockItem();
            UpdateUI();
        }
    }
}
