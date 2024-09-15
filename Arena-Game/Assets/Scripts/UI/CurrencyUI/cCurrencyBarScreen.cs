using System.Collections.Generic;
using ArenaGame.Managers.SaveManager;
using UnityEngine;

namespace ArenaGame.UI.Currency
{
    public class cCurrencyBarScreen : MonoBehaviour
    {
        List<cCurrencyBar> m_CurrencyBars = new List<cCurrencyBar>();
        private ISaveManager m_SaveManager;

        public void Init(ISaveManager saveManager)
        {
            m_SaveManager = saveManager;
        }

        public int CurrentCurrencyAmount
        {
            get => m_SaveManager.SaveData.m_Currency;
            set
            {
                m_SaveManager.SaveData.m_Currency = value;
                // PlayerMaxScore = value;
            }
        }
    
        // public int PlayerMaxScore
        // {
        //     get =>  m_SaveManager.SaveData.m_MaxCoinCount;
        //     set
        //     {
        //         if (value > PlayerMaxScore)
        //         {
        //             m_SaveManager.SaveData.m_MaxCoinCount = value;
        //         }
        //     }
        // }

        public void RegisterBar(cCurrencyBar bar)
        {
            m_CurrencyBars.Add(bar);
        }
    
        public void SpendCurrency(int currency)
        {
            CurrentCurrencyAmount -= currency;
            m_SaveManager.Save();
        
            foreach (var bar in m_CurrencyBars)
            {
                bar.Refresh(CurrentCurrencyAmount);
            }
        }
    
        public void GainCurrency(int currency)
        {
            CurrentCurrencyAmount += currency;
            m_SaveManager.Save();
        
            foreach (var bar in m_CurrencyBars)
            {
                bar.Refresh(CurrentCurrencyAmount);
            }
        }
    
        public void Refresh(int currency)
        {
            foreach (var bar in m_CurrencyBars)
            {
                bar.Refresh(currency);
            }
        }
    }
}