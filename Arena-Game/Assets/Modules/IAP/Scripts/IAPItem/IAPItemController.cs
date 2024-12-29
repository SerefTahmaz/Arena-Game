using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Managers.SaveManager;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using TMPro;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.Purchasing;

 

public class IAPItemController : MonoBehaviour
{
    [SerializeField] private cButton m_Button;
    [SerializeField] private bool m_DisableIfAlreadyPurchased;
    [SerializeField] private BaseIAPItemSO m_IAPItemSO;
    [SerializeField] private TMP_Text m_PriceText;
    
    private Product m_Product;
    private string ProductId => m_IAPItemSO.productId;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Product = IAPManager.Instance.GetProduct(ProductId);
        if (m_Product == null) 
        {
            gameObject.SetActive(false);
            return;
        }

        if (m_IAPItemSO.PurchaseCount() > 0 && m_DisableIfAlreadyPurchased)
        {
            gameObject.SetActive(false);
            return;
        }
         
        m_PriceText.SetText($"{m_Product.metadata.localizedPriceString}" +$" {m_Product.metadata.isoCurrencyCode}");
        m_Button.OnClickEvent.AddListener(HandleButtonClicked);
    }

    private void HandleButtonClicked()
    {
        HandlePurchase();
    }

    private async UniTask HandlePurchase()
    {
        m_Button.enabled = false;
        var result = await IAPManager.Instance.HandlePurchase(m_Product);
        switch (result)
        {
            case RequestResult.Failed:
                break;
            case RequestResult.Success:
                GiveReward();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        m_Button.enabled = true;
    }

    private void GiveReward()
    {
        m_IAPItemSO.GiveReward();
    }
}

 
