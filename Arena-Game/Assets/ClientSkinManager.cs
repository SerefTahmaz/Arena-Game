using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Managers.SaveManager;
using DefaultNamespace;
using Gameplay.Item;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class ClientSkinManager : NetworkBehaviour
{
    [SerializeField] private SkinManager m_SkinManager;

    private NetworkVariable<FixedString512Bytes> m_HeadGuid = 
        new NetworkVariable<FixedString512Bytes>("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<FixedString512Bytes> m_ChestGuid = 
        new NetworkVariable<FixedString512Bytes>("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<FixedString512Bytes> m_GaunletsGuid = 
        new NetworkVariable<FixedString512Bytes>("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<FixedString512Bytes> m_LeggingGuid = 
        new NetworkVariable<FixedString512Bytes>("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            m_SkinManager.m_OnClearEquip += OnClearEquip;
            m_SkinManager.m_OnEquipItem += OnEquipItem;
            m_SkinManager.Init();
        }
        else
        {
            ValueChanged("",m_HeadGuid.Value);
            ValueChanged("",m_ChestGuid.Value);
            ValueChanged("",m_GaunletsGuid.Value);
            ValueChanged("",m_LeggingGuid.Value);
            
            m_HeadGuid.OnValueChanged += ValueChanged;
            m_ChestGuid.OnValueChanged += ValueChanged;
            m_GaunletsGuid.OnValueChanged += ValueChanged;
            m_LeggingGuid.OnValueChanged += ValueChanged;
        }
    }

    private void ValueChanged(FixedString512Bytes previousvalue, FixedString512Bytes newvalue)
    {
        if (string.IsNullOrEmpty(newvalue.Value) && !string.IsNullOrEmpty(previousvalue.Value))
        {
            var armorItemTemplate = ItemListSO.GetItemByGuid<ArmorItemTemplate>(previousvalue.Value);
            OnClearEquipClient(armorItemTemplate.ArmorType);
        }
        else if (string.IsNullOrEmpty(newvalue.Value))
        {
        }
        else 
        {
            OnEquipItemClient(newvalue.Value);
        }
    }

    private void OnEquipItem(ArmorItemSO armorItemSo)
    {
        switch (armorItemSo.ArmorType)
        {
            case ArmorType.Helm:
                m_HeadGuid.Value = armorItemSo.ItemTemplate.Guid.ToHexString();
                break;
            case ArmorType.Chest:
                m_ChestGuid.Value = armorItemSo.ItemTemplate.Guid.ToHexString();
                break;
            case ArmorType.Gauntlets:
                m_GaunletsGuid.Value = armorItemSo.ItemTemplate.Guid.ToHexString();
                break;
            case ArmorType.Legging:
                m_LeggingGuid.Value = armorItemSo.ItemTemplate.Guid.ToHexString();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public void OnEquipItemClient(string guid)
    {
        if(IsOwner) return;
        
        Debug.Log($"Client Equipping guid {guid}");
        var armorItemTemplate = ItemListSO.GetItemByGuid<ArmorItemTemplate>(guid);
        var armorItemSO = ScriptableObject.CreateInstance<ArmorItemSO>();
        armorItemSO.ItemTemplate = armorItemTemplate;
        m_SkinManager.EquipItem(armorItemSO);
    }

    private void OnClearEquip(ArmorType armorType)
    {
        switch (armorType)
        {
            case ArmorType.Helm:
                m_HeadGuid.Value = "";
                break;
            case ArmorType.Chest:
                m_ChestGuid.Value = "";
                break;
            case ArmorType.Gauntlets:
                m_GaunletsGuid.Value = "";
                break;
            case ArmorType.Legging:
                m_LeggingGuid.Value = "";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void OnClearEquipClient(ArmorType armorType)
    {
        if(IsOwner) return;
        m_SkinManager.ClearEquip(armorType);
    }

    private void Update()
    {
        if(!IsOwner) return;
    }
}
