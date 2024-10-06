using System.Collections;
using System.Collections.Generic;
using ArenaGame.Currency;
using ArenaGame.Experience;
using UnityEngine;

public class SaveDebugButton : MonoBehaviour
{
    public void AddCurrency()
    {
        CurrencyManager.GainCurrency(100000);
        ExperienceManager.GainExperience(100000);
    }

    public void ClearCurrency()
    {
        CurrencyManager.SpendCurrency(CurrencyManager.Currency());
        ExperienceManager.LoseExperience(ExperienceManager.Experience());
    }
}
