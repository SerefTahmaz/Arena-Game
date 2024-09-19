using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketItemController : MonoBehaviour
{
    [SerializeField] private MarketItemSO m_MarketItemSo;
    [SerializeField] private Image m_Image;
    [SerializeField] private TMP_Text m_Price;
    [SerializeField] private GameObject m_EquipedLayer;
    [SerializeField] private CharacterSO m_CharacterSo;

    private bool IsUnlocked => m_MarketItemSo.IsUnlocked();

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        m_Image.sprite = m_MarketItemSo.RewardItem.ItemSprite;
        
        if (IsUnlocked)
        {
            m_Price.text = "";
        }
        else
        {
            m_Price.text = m_MarketItemSo.Price.ToString();
        }
        
        m_EquipedLayer.SetActive(m_CharacterSo.IsItemEquiped(m_MarketItemSo.RewardItem));
    }
    
    public void Buy()
    {
        BuyAsync();
    }

    private async UniTask BuyAsync()
    {
        if (m_MarketItemSo.IsUnlocked())
        {
            HandleEquipment();
        }
        else
        {
            var popUp = GameFactorySingleton.Instance.PurchasePopUpFactory.Create();
            var result = await popUp.Init(m_MarketItemSo.RewardItem.ItemName, m_MarketItemSo.Price.ToString());

            if (result)
            {
                m_MarketItemSo.UnlockItem();
                HandleEquipment();
            }
            else
            {
                Debug.Log("Purchase Canceled!");
            }
        }
    }

    private void HandleEquipment()
    {
        m_CharacterSo.Load();
        var rewardItem = m_MarketItemSo.RewardItem as ArmorItem;
        if(m_CharacterSo.IsItemEquiped(rewardItem))
        {
            m_CharacterSo.EquipmentList.Remove(rewardItem);
            m_EquipedLayer.SetActive(false);
        }
        else
        {
            m_CharacterSo.EquipmentList.Add(rewardItem);
            m_EquipedLayer.SetActive(true);
        }
        m_CharacterSo.Save();
    }
}
