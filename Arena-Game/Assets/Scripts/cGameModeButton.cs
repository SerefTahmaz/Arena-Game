using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class cGameModeButton : MonoBehaviour
{
    [SerializeField] private TMP_Text m_Text;
    private cLobbyManager.GameMode m_GameMode = cLobbyManager.GameMode.PvP;

    public cLobbyManager.GameMode GameMode => m_GameMode;

    private void Awake()
    {
        UpdateUI();
    }

    public void OnClick()
    {
        m_GameMode = (cLobbyManager.GameMode) ((int)(GameMode+1) % Enum.GetNames(typeof(cLobbyManager.GameMode)).Length);
        UpdateUI();
    }

    private void UpdateUI()
    {
        m_Text.text = GameMode.ToString();
    }
}
