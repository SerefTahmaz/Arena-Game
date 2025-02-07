﻿using System;
using Item;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UI.Shop
{
    [CreateAssetMenu(fileName = "Item", menuName = "MarketItem", order = 0)]
    public class MarketItemSO : SerializableScriptableObject
    {
        [SerializeField] private BaseItemSO rewardItemTemplate;
        [SerializeField] private int m_Price;
        
        public int Price
        {
            get => m_Price;
            set => m_Price = value;
        }

        public BaseItemSO RewardItemTemplate
        {
            get => rewardItemTemplate;
            set => rewardItemTemplate = value;
        }
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(MarketItemSO))]
    public class MarketItemSOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("FixName"))
            {
                var marketItemSO = target as MarketItemSO;
                if (marketItemSO.RewardItemTemplate)
                {
                    AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(target), $"{marketItemSO.RewardItemTemplate.ItemName} Market Item");

                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
        }
    }
#endif
}