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
        [SerializeField] private cButton m_PurchaseButton;
        [SerializeField] private cButton m_PreviewButton;
        [SerializeField] private GameObject m_PreviewLayer;
    
        private MarketItemSO m_MarketItemSo;
        private IMarketItemHandler m_MarketItemHandler;
        private BaseItemSO m_RewardItem;

        public MarketItemSO MarketItemSo => m_MarketItemSo;

        public bool IsPreviewing
        {
            get => m_IsPreviewing;
            set => m_IsPreviewing = value;
        }

        public BaseItemSO RewardItem => m_RewardItem;

        public void Init(MarketItemSO marketItemSo, IMarketItemHandler marketItemHandler)
        {
            m_MarketItemSo = marketItemSo;
            m_MarketItemHandler = marketItemHandler;
            m_RewardItem = marketItemSo.RewardItemTemplate.DuplicateUnique();
        
            m_Image.sprite = m_RewardItem.ItemSprite;

            UpdateUI();
            
            m_PurchaseButton.OnClickEvent.AddListener(Buy);
            m_PreviewButton.OnClickEvent.AddListener(PreviewItem);
        }

        public void UpdateUI()
        {
            m_Price.text = MarketItemSo.Price.ToString();
        }
    
        public void Buy()
        {
            BuyAsync();
        }

        private async UniTask BuyAsync()
        {
            m_MarketItemHandler.HandleBuy(this);
        }

        private bool m_IsPreviewing;

        private void PreviewItem()
        {
            m_MarketItemHandler.HandlePreview(this);
        }

        private void HandlePurchase()
        {
            m_MarketItemHandler.HandlePurchase(this);
        }

        public void DecreaseAmount()
        {
            m_RewardItem = MarketItemSo.RewardItemTemplate.DuplicateUnique();
            UpdateUI();
        }

        public void SetPreviewState(bool value)
        {
            m_IsPreviewing = value;
            m_PreviewLayer.SetActive(value);
        }
    }
}
