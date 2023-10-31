using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class cVisibilityButton : MonoBehaviour
{
    [SerializeField] private TMP_Text m_Text;
    private cLobbyManager.Visibility m_Visibility = cLobbyManager.Visibility.Public;

    public bool isPrivate => m_Visibility == cLobbyManager.Visibility.Private ? true : false;

    private void Awake()
    {
        UpdateUI();
    }

    public void OnClick()
    {
        m_Visibility = (cLobbyManager.Visibility) ((int)(m_Visibility+1) % Enum.GetNames(typeof(cLobbyManager.Visibility)).Length);
        UpdateUI();
    }

    private void UpdateUI()
    {
        m_Text.text = m_Visibility.ToString();
    }
}
