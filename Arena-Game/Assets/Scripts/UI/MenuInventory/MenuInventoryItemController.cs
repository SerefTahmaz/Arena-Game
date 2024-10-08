using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.UI.MenuInventory;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Gameplay.Item;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuInventoryItemController : MonoBehaviour
{
    [SerializeField] protected GameObject m_EquipedLayer;
    [SerializeField] private cButton m_Button;

    private IMenuInventoryItemHandler m_MenuInventoryItemHandler;

    public void Init(IMenuInventoryItemHandler menuInventoryItemHandler)
    {
        m_MenuInventoryItemHandler = menuInventoryItemHandler;
        
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
