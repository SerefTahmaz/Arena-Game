using System;
using System.Collections;
using System.Collections.Generic;
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
            foreach (var item in itemGroup.m_Items)
            {
                var value = Random.value;
                if (item.m_Possibilty >= value)
                {
                    Debug.Log(value);
                    item.m_Item.SetActive(true);
                    if(itemGroup.m_IsExclusive) break;
                }
            }
        }
    }
}
