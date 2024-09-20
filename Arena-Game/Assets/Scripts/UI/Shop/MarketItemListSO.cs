using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace UI.Shop
{
    [CreateAssetMenu(fileName = "MarketItemList", menuName = "Game/MarketItemList", order = 0)]
    public class MarketItemListSO : ScriptableObject
    {
        [SerializeField] private List<MarketItemSO> m_MarketItemSOs;

        public List<MarketItemSO> MarketItemSOs => m_MarketItemSOs;
    }
}