using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Gameplay.Item;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuInventoryItemController : MonoBehaviour
{
    [SerializeField] private Image m_Image;
    [SerializeField] private GameObject m_EquipedLayer;
    [SerializeField] private cButton m_Button;

    public ArmorItemSO itemTemplate;
    private IMenuInventoryItemHandler m_MenuInventoryItemHandler;

    public void Init(ArmorItemSO itemTemplate, bool isWearing, IMenuInventoryItemHandler menuInventoryItemHandler)
    {
        this.itemTemplate = itemTemplate;
        m_MenuInventoryItemHandler = menuInventoryItemHandler;
        
        m_Image.sprite = itemTemplate.ItemTemplate.ItemSprite;

        if (isWearing)
        {
            m_EquipedLayer.SetActive(true);
        }
        
        m_Button.OnClickEvent.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        Debug.Log("Clicked");
        m_MenuInventoryItemHandler.HandleClick(this);
    }

    public void SetEquipState(bool isItemEquiped)
    {
        m_EquipedLayer.SetActive(isItemEquiped);
    }
}
