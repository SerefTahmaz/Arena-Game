using System;
using System.Collections.Generic;
using DefaultNamespace;
using Gameplay.Farming;
using Gameplay.Item;
using Item;

namespace ArenaGame.Managers.SaveManager
{
    [Serializable]
    public class SaveData
    {
        public string m_PlayerName = "NewPlayer";
        public bool m_IsPlayerSetName;
        public int m_Currency;
        public int m_ExperiencePoint;
        public int m_WinsCount;
        public bool m_AudioState = true;
        public bool m_MusicState = true;
        public bool m_HapticState = true;
        public int m_CurrentPVELevel;
        public int m_CurrentMap;
        public bool m_IsPlayerDisqualified;
    }

    [Serializable]
    public class CharacterData
    {
        public Dictionary<string, Character> Characters = new Dictionary<string, Character>();
    }

    [Serializable]
    public class Character
    {
        public int Health = 100;
        public int Currency = 0;
        public List<string> InventoryList = new List<string>();
        
        public string HelmArmor;
        public string ChestArmor;
        public string GaunletsArmor;
        public string LeggingArmor;
    }
    
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