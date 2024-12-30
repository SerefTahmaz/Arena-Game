using System;
using System.Collections.Generic;
using ArenaGame.Managers.SaveManager;
using DefaultNamespace;

using UnityEngine;
using UnityEngine.Purchasing;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Purchasing;
#endif

namespace DefaultNamespace
{
    public class BaseIAPItemSO : ScriptableObject
    {
        [HideInInspector] public string productId;
        
        public string DatabaseKey => productId.Replace('.', ',');
        public Action OnRewardGiven { get; set; }
         
        public virtual void GiveReward()
        {
            UtilitySaveHandler.SaveData.m_Purchases.TryAdd(DatabaseKey, 0);
            UtilitySaveHandler.SaveData.m_Purchases[DatabaseKey]++;
            Debug.Log($"Product id {productId}");
            UtilitySaveHandler.Save();
            OnRewardGiven?.Invoke();
        }

        public int PurchaseCount()
        {
            UtilitySaveHandler.SaveData.m_Purchases.TryGetValue(DatabaseKey, out var count);
            return count;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(BaseIAPItemSO),true)]
public class BaseIAPItemSOEditor : Editor
{
    private const string kNoProduct = "<None>";

    private readonly List<string> m_ValidIDs = new List<string>();
    private SerializedProperty m_ProductIDProperty;

    private void OnEnable()
    {
        m_ProductIDProperty = serializedObject.FindProperty("productId");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var productId = ((BaseIAPItemSO)target).productId;
        DrawProductIdDropDown(true, productId);
    }

    void DrawProductIdDropDown(bool isAPurchaseButton, string productId)
    {
        serializedObject.Update();

        DrawProductIdDropdownWhenButtonIsPurchaseType(isAPurchaseButton, productId);

        // DrawPropertiesExcluding(serializedObject);

        serializedObject.ApplyModifiedProperties();
    }

    void DrawProductIdDropdownWhenButtonIsPurchaseType(bool isAPurchaseButton, string productId)
    {
        if (isAPurchaseButton)
        {
            EditorGUILayout.LabelField(new GUIContent("Product ID:", "Select a product from the IAP catalog."));
            LoadProductIdsFromCodelessCatalog();
            m_ProductIDProperty.stringValue = GetCurrentlySelectedProduct(productId);
            
            if (GUILayout.Button("IAP Catalog..."))
            {
                ProductCatalogEditor.ShowWindow();
            }
        }
    }

    void LoadProductIdsFromCodelessCatalog()
    {
        var catalog = ProductCatalog.LoadDefaultCatalog();

        m_ValidIDs.Clear();
        m_ValidIDs.Add(kNoProduct);
        foreach (var product in catalog.allProducts)
        {
            m_ValidIDs.Add(product.id);
        }
    }

    string GetCurrentlySelectedProduct(string productId)
    {
        var currentIndex = string.IsNullOrEmpty(productId) ? 0 : m_ValidIDs.IndexOf(productId);
        var newIndex = EditorGUILayout.Popup(currentIndex, m_ValidIDs.ToArray());
        return newIndex > 0 && newIndex < m_ValidIDs.Count ? m_ValidIDs[newIndex] : string.Empty;
    }
}
#endif