using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class cPlayerCountButton : MonoBehaviour
{
    [SerializeField] private TMP_Text m_Text;
    private int m_PlayerCount = 4;

    public int PlayerCount
    {
        get => m_PlayerCount;
        set => m_PlayerCount = value;
    }

    private void Awake()
    {
        UpdateUI();
    }

    public void IncreasePlayerCount()
    {
        PlayerCount = Mathf.Clamp(m_PlayerCount + 1, 1, 4);
        UpdateUI();
    }
    public void DecreasePlayerCount()
    {
        PlayerCount = Mathf.Clamp(m_PlayerCount - 1, 1, 4);
        UpdateUI();
    }
    private void UpdateUI()
    {
        m_Text.text = m_PlayerCount.ToString();
    }
}
