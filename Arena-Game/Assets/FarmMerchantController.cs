using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Factories;
using Gameplay;
using UI.Shop;
using UnityEngine;

public class FarmMerchantController : MonoBehaviour
{
    [SerializeField] private CharacterSO m_FarmerChar;
    [SerializeField] private List<BaseItemSO> m_FarmerMarketSOs;
    [SerializeField] private GameObject m_FocusCam;
    [SerializeField] private PlayerDetector m_PlayerDetector;

    private ITransactionShopPopUpController m_InsTransactionShopPopUpController;

    private void Start()
    {
        m_PlayerDetector.OnPlayerEntered += HandleOnPlayerEntered;
    }

    private void HandleOnPlayerEntered()
    {
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
    }

    private void OnTransactionDismissed()
    {
        m_InsTransactionShopPopUpController.OnDismissed -= OnTransactionDismissed;
        m_FocusCam.SetActive(false);
        m_InsTransactionShopPopUpController = null;
    }
}
