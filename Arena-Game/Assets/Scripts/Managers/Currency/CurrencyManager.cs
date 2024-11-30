using System;
using ArenaGame.Managers.SaveManager;
using ArenaGame.Utils;
using DefaultNamespace;
using UnityEngine;

namespace ArenaGame.Currency
{
    public class CurrencyManager
    {
        public static int Currency()
        {
            SaveGameHandler.Load();
            return GameplayStatics.GetPlayerCharacterSO().GetCharacterSave().Currency;
        }
        
        public static void GainCurrency(int amount)
        {
            var savaData = GameplayStatics.GetPlayerCharacterSO();
            savaData.GetCharacterSave().GainCurrency(amount);
        }

        public static void SpendCurrency(int amount)
        {
            var savaData = GameplayStatics.GetPlayerCharacterSO();
            savaData.GetCharacterSave().SpendCurrency(amount);
        }

        public static bool HasEnoughCurrency(int amount)
        {
            return Currency() >= amount;
        }
    }
}