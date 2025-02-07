using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cReadyButtonController : MonoBehaviour
{
    [SerializeField] private GameObject m_SelectedOverlay;
    [SerializeField] private cButton m_Button;
    
    private bool m_isReady = false;

    public Action<bool> OnReadyStateChange;

    public bool IsReady => m_isReady;

    private void Awake()
    {
        m_Button.OnClickEvent.AddListener(OnClick);
    }

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
        m_SelectedOverlay.SetActive(IsReady);
    }
}
