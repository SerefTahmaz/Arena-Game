using System.Collections.Generic;
using AudioSystem;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Gameplay;
using Gameplay.Item;
using UnityEngine;

namespace UI.Shop
{
    public class ShopController : MonoBehaviour,IMarketItemHandler
    {
        [SerializeField] private MarketItemController m_MarketItemPrefab;
        [SerializeField] private Transform m_LayoutParent;
        [SerializeField] protected MarketItemListSO m_MarketItemListSo;
        [SerializeField] private AudioClip m_PurchaseClip;
        
        protected List<MarketItemController> m_MarketItemControllers = new List<MarketItemController>();

        public virtual void Init()
        {
        }

        public virtual void Refresh()
        {
            foreach (var VARIABLE in m_MarketItemControllers)
            {
                Destroy(VARIABLE.gameObject);
            }
            m_MarketItemControllers.Clear();
            foreach (var marketItemSo in m_MarketItemListSo.MarketItemSOs)
            {
                var ins = Instantiate(m_MarketItemPrefab,m_LayoutParent);
                ins.Init(marketItemSo,this);
                m_MarketItemControllers.Add(ins);
            }
        }

        public virtual async UniTask HandleBuy(MarketItemController marketItemController)
        {
            
        }

        public virtual void HandlePurchase(MarketItemController marketItemController)
        {
            SoundManager.PlayOneShot2DSFX(m_PurchaseClip);
        }

        public virtual void HandlePreview(MarketItemController marketItemController)
        {
          
        }
    }

    public interface IMarketItemHandler
    {
        UniTask HandleBuy(MarketItemController marketItemController);
        void HandlePurchase(MarketItemController marketItemController);
        void HandlePreview(MarketItemController marketItemController);
    }
}