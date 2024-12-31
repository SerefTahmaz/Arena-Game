using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Managers.SaveManager;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Purchasing;

public class NoAdsMenuButton : MonoBehaviour
{
    [SerializeField] private NoAdsIAPItemSO m_NoAdsIAPItemSo;
    [SerializeField] private cButton m_Button;
    [SerializeField] private ChoicePopUpUIController m_ChoicePopUpUIController;
    
    private Product m_Product;
     
    // Start is called before the first frame update
    void Start()
    {
        if (UtilitySaveHandler.SaveData.m_NoAdsPurchased)
        {
            gameObject.SetActive(false);
            return;
        }
        m_Product = IAPManager.Instance.GetProduct(m_NoAdsIAPItemSo.productId);
        if (m_Product == null) 
        {
            gameObject.SetActive(false);
            return;
        }
        
        m_Button.OnClickEvent.AddListener(HandleButtonClick);
        m_NoAdsIAPItemSo.OnRewardGiven += OnRewardGiven;
    }

    private void OnDestroy()
    {
        m_NoAdsIAPItemSo.OnRewardGiven -= OnRewardGiven;
    }

    private void OnRewardGiven()
    {
        gameObject.SetActive(false);
        m_NoAdsIAPItemSo.OnRewardGiven -= OnRewardGiven;
    }

    private void HandleButtonClick()
    {
        InitNoAdsPopUp();
    }

    private async UniTask InitNoAdsPopUp()
    {
        m_Button.enabled = false;
        var popUp = ChoicePopUpUIController.CreateFromInstance(m_ChoicePopUpUIController);
        var isSuccess = await popUp.Init("Remove Ads from the game");
        if (isSuccess)
        {
            await HandlePurchase();
        }
        m_Button.enabled = true;
    }
    
    private async UniTask HandlePurchase()
    {
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
    }
    
    private void GiveReward()
    {
        m_NoAdsIAPItemSo.GiveReward();
    }

}
