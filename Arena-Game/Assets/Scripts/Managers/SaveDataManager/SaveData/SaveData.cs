using System;
using System.Collections.Generic;

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
        public int m_CurrentLevel;
    }

    [Serializable]
    public class CharacterData
    {
        public Dictionary<string, Character> Characters = new Dictionary<string, Character>();
    }
    
    [Serializable]
    public class ItemData
    {
        public Dictionary<string, SaveableArmorItem> Items = new Dictionary<string, SaveableArmorItem>();
    }

    public class Character
    {
        public int Health = 100;
        public List<string> InventoryList = new List<string>();
        
        public string HelmArmor;
        public string ChestArmor;
        public string GaunletsArmor;
        public string LeggingArmor;
    }
}