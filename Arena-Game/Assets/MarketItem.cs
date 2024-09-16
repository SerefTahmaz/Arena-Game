using System;
using Item;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "Item", menuName = "MarketItem", order = 0)]
    public class MarketItem : SerializableScriptableObject
    {
        [SerializeField] private BaseItemSO m_RewardItem;
        [SerializeField] private int m_Price;
        
        public int Price => m_Price;

        public void UnlockItem()
        {
            PlayerPrefs.SetString(Guid.ToHexString(),"true");
        }

        public bool IsUnlocked()
        {
            if (PlayerPrefs.GetString(Guid.ToHexString()) == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}