using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cReadyButtonController : MonoBehaviour
{
    [SerializeField] private Image m_Image;
    
    private bool m_isReady = false;

    public Action<bool> OnReadyStateChange;

    public bool IsReady => m_isReady;

    public void OnClick()
    {
        m_isReady = !IsReady;
        UpdateReadyState();
    }

    public void ResetState()
    {
        m_isReady = false;
        UpdateUI();
    }
    
    private void UpdateReadyState()
    {
        OnReadyStateChange?.Invoke(IsReady);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (IsReady)
        {
            m_Image.color = Color.green;
        }
        else
        {
            m_Image.color = Color.gray;
        }
    }
}
