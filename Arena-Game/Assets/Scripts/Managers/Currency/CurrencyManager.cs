using System;
using ArenaGame.Managers.SaveManager;
using ArenaGame.Utils;
using UnityEngine;

namespace ArenaGame.Currency
{
    public class CurrencyManager
    {
        public static int Currency()
        {
            SaveGameHandler.Load();
            return SaveGameHandler.SaveData.m_Currency;
        }
        
        public static void GainCurrency(int amount)
        {
            SaveGameHandler.Load();
            var savaData = SaveGameHandler.SaveData;
            savaData.m_Currency += amount;
            SaveGameHandler.Save();
        }

        public static void SpendCurrency(int amount)
        {
            SaveGameHandler.Load();
            var savaData = SaveGameHandler.SaveData;
            savaData.m_Currency -= amount;

            if (savaData.m_Currency < 0)
            {
                Debug.Log("Currency cant be less than zero!!!!");
            }
            SaveGameHandler.Save();
        }

        public static bool HasEnoughCurrency(int amount)
        {
            return Currency() >= amount;
        }
    }
}