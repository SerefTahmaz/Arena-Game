using System;
using System.Collections.Generic;

namespace ArenaGame.Managers.SaveManager
{
    [Serializable]
    public class CharacterData
    {
        public int Health = 100;
        public int Currency = 0;
        public List<string> InventoryList = new List<string>();
        
        public string HelmArmor;
        public string ChestArmor;
        public string GaunletsArmor;
        public string LeggingArmor;
    }
}