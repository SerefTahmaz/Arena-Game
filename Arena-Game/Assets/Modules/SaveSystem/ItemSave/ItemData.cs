using System;
using System.Collections.Generic;
using DefaultNamespace;
using Gameplay.Farming;
using Gameplay.Item;
using Item;

namespace ArenaGame.Managers.SaveManager
{
    [Serializable]
    public class ItemData
    {
        public Dictionary<string, ArmorItem> ArmorItems = new Dictionary<string, ArmorItem>();
        public Dictionary<string, PlantItem> PlantItems = new Dictionary<string, PlantItem>();
        public Dictionary<string, PlantFieldItem> PlantFieldItems = new Dictionary<string, PlantFieldItem>();
        public Dictionary<string, SeedItem> SeedItems = new Dictionary<string, SeedItem>();
        public Dictionary<string, FoodItem> FoodItems = new Dictionary<string, FoodItem>();
        public Dictionary<string, Dictionary<string,string>> WeaponItems = new Dictionary<string, Dictionary<string,string>>();
    }
    
    [Serializable]
    public class ArmorItem
    {
        public string m_ArmorItemTemplateGUID;
        public string m_ItemName;
        public ItemType m_ItemType;
        public int m_Level;
        public int m_NextLevelIncrement;
    }

    [Serializable]
    public class PlantItem
    {
        public string m_ItemName;
        public ItemType m_ItemType;
        public string m_PlantItemTemplate;
        public DateTime m_CreationDate;
        public PlantState m_PlantState;
    }

    [Serializable]
    public class PlantFieldItem
    {
        public List<string> m_PlantItems = new List<string>();
    }
    
    [Serializable]
    public class SeedItem
    {
        public string m_SeedItemTemplateGUID;
        public string m_ItemName;
        public ItemType m_ItemType;
    }
    
    [Serializable]
    public class FoodItem
    {
        public string m_FoodItemTemplateGUID;
        public string m_ItemName;
        public ItemType m_ItemType;
    }
}