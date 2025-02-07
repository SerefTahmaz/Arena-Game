using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.UI;
using UnityEngine;
using UnityEngine.Events;

public class cGameplayMenuUIController : MonoBehaviour
{
    [SerializeField] private cView m_MenuView;
    [SerializeField] private Transform m_ScoreBoardUITransform;
    
    public UnityEvent OnActivateEvent { get; set; }

    public Transform ScoreBoardUITransform => m_ScoreBoardUITransform;

    public void OnClick()
    {
        if (m_MenuView.m_IsActive)
        {
            m_MenuView.Deactivate();
        }
        else
        {
            m_MenuView.Activate();
            OnActivateEvent?.Invoke();
        }
    }

    public void DeactivateView()
    {
        m_MenuView.Deactivate();
    }
}
