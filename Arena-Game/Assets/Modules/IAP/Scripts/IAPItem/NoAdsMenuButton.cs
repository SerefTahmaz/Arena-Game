using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Managers.SaveManager;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using UnityEngine;

public class NoAdsMenuButton : MonoBehaviour
{
    [SerializeField] private NoAdsIAPItemSO m_NoAdsIAPItemSo;
    [SerializeField] private cButton m_Button;
    [SerializeField] private ChoicePopUpUIController m_ChoicePopUpUIController;
     
    // Start is called before the first frame update
    void Start()
    {
        if (UtilitySaveHandler.SaveData.m_NoAdsPurchased)
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
        var popUp = ChoicePopUpUIController.CreateFromInstance(m_ChoicePopUpUIController);
        var isSuccess = await popUp.Init("Remove Ads from game");
        if (isSuccess)
        {
            m_NoAdsIAPItemSo.GiveReward();
        }
    }
}
