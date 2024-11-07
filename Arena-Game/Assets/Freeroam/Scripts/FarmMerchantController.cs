using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Factories;
using Gameplay;
using UI.Shop;
using UnityEngine;

public class FarmMerchantController : InteractableNPC
{
    [SerializeField] private CharacterSO m_FarmerChar;
    [SerializeField] private List<BaseItemSO> m_FarmerMarketSOs;
    [SerializeField] private GameObject m_FocusCam;
    [SerializeField] private DialogHelper m_DialogHelper;

    private ITransactionShopPopUpController m_InsTransactionShopPopUpController;

    private bool m_IsShowingInventory;

    protected override void Start()
    {
        base.Start();
    }

    protected override async UniTask  StartInteraction()
    {
        m_DialogHelper.ShowMerchantInventory += ShowInventory;
        await base.StartInteraction();
    }
    
    private void ShowInventory()
    {
        ShowInventoryAsync();
    }

    private async UniTask ShowInventoryAsync()
    {
        m_DialogHelper.ShowMerchantInventory -= ShowInventory;
        m_IsShowingInventory = true;
        if (m_DialogController)
        {
            m_DialogController.SetVisibility(false);
        }
        
        m_FocusCam.SetActive(true);
            
        m_FarmerChar.Load();
        m_FarmerChar.InventoryList = new List<BaseItemSO>();
        m_FarmerChar.Save();
        m_FarmerChar.GainCurrency(999999);
        var items = m_FarmerMarketSOs.Select((so => so.DuplicateUnique()));
        foreach (var VARIABLE in  items)
        {
            m_FarmerChar.AddInventory(VARIABLE);
            m_FarmerChar.Save();
        }
        m_InsTransactionShopPopUpController = GlobalFactory.TransactionShopPopUpFactory.Create();
        m_InsTransactionShopPopUpController.Init(GameplayStatics.GetPlayerCharacterSO(), m_FarmerChar);
        m_InsTransactionShopPopUpController.OnDismissed += OnTransactionDismissed;

        await UniTask.WaitWhile((() => m_IsShowingInventory));
        m_DialogHelper.ShowMerchantInventory += ShowInventory;
    }

    private void OnTransactionDismissed()
    {
        m_InsTransactionShopPopUpController.OnDismissed -= OnTransactionDismissed;
        m_FocusCam.SetActive(false);
        m_InsTransactionShopPopUpController = null;
        if (m_DialogController)
        {
            m_DialogController.SetVisibility(true);
        }
        m_IsShowingInventory = false;
    }

    protected override void HandleOnDialogEnded()
    {
        base.HandleOnDialogEnded();
        m_DialogHelper.ShowMerchantInventory -= ShowInventory;
    }
}
