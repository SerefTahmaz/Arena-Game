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
    

    private void Awake()
    {
        m_Button.OnClickEvent.AddListener(HandleOK);
    }
        
    private void HandleOK()
    {
        var currentMap = MapListSO.GetCurrentMap();
        
    }
}
