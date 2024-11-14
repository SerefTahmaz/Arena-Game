using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArenaGame.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ItemRandomizer : MonoBehaviour
{
    [SerializeField] private List<ItemGroup> m_ItemGroups;
    
    [Serializable]
    public class ItemWrapper
    {
        public GameObject m_Item;
        [Range(0,1)] public float m_Possibilty;
    }
    [Serializable]
    public class ItemGroup
    {
        public List<ItemWrapper> m_Items;
        public bool m_IsExclusive;
        public bool m_IsOneHasToBeSeleceted;
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var VARIABLE in m_ItemGroups)
        {
            foreach (var item in VARIABLE.m_Items)
            {
                item.m_Item.SetActive(false);
            }
        }

        foreach (var itemGroup in m_ItemGroups)
        {
            bool oneSelected = false;
           
            foreach (var item in itemGroup.m_Items)
            {
                var value = Random.value;
                if (item.m_Possibilty >= value)
                {
                    oneSelected = true;
                    item.m_Item.SetActive(true);
                    if(itemGroup.m_IsExclusive) break;
                }
            }

            if (itemGroup.m_IsOneHasToBeSeleceted && !oneSelected)
            {
                itemGroup.m_Items.RandomItem().m_Item.SetActive(true);
            }
            
        }
    }
}
