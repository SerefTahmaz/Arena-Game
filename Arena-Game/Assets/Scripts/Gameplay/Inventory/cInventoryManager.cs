using System;
using System.Collections.Generic;
using UnityEngine;

public class cInventoryManager : MonoBehaviour
{
    [SerializeField] private List<cInventoryItem> m_InventoryItems;

    public void InitInventory(int teamID)
    {
        foreach (var VARIABLE in m_InventoryItems)
        {
            VARIABLE.TeamId = teamID;
        }
    }

    public bool IsMyItem(cInventoryItem inventoryItem)
    {
        return m_InventoryItems.Contains(inventoryItem);
    }
}