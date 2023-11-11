using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class cGameModeButton : MonoBehaviour
{
    [SerializeField] private TMP_Text m_Text;
    private eGameMode m_GameMode = eGameMode.PvP;

    public eGameMode GameMode => m_GameMode;

    private void Awake()
    {
        UpdateUI();
    }

    public void OnClick()
    {
        m_GameMode = (eGameMode) ((int)(GameMode+1) % Enum.GetNames(typeof(eGameMode)).Length);
        UpdateUI();
    }

    private void UpdateUI()
    {
        m_Text.text = GameMode.ToString();
    }
}
