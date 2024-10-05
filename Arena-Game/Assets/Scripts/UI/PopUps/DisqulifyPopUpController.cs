using System.Collections;
using System.Collections.Generic;
using ArenaGame.Currency;
using ArenaGame.Experience;
using ArenaGame.Managers.SaveManager;
using TMPro;
using UnityEngine;

public class DisqulifyPopUpController : MonoBehaviour
{
    [SerializeField] private cButton m_Button;
    [SerializeField] private TMP_Text m_LoseExp;
    [SerializeField] private TMP_Text m_LoseCurrency;

    private void Awake()
    {
        m_Button.OnClickEvent.AddListener(HandleOK);
        
        var currentMap = MapListSO.GetCurrentMap();
        ExperienceManager.LoseExperience(currentMap.LoseExp);
        CurrencyManager.SpendCurrency(currentMap.LoseCurrency);

        m_LoseExp.text = "-"+currentMap.LoseExp.ToString();
        m_LoseCurrency.text = "-"+currentMap.LoseCurrency.ToString();
    }
        
    private void HandleOK()
    {
        
    }
}
