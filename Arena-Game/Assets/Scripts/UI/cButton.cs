using System;
using ArenaGame.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class cButton : MonoBehaviour
{
    [SerializeField] private CanvasGroup m_CanvasGroup;
    [SerializeField] private Image m_BGImage;
    [SerializeField] private UnityEvent m_OnClick;

    private Color m_StartColor;

    public UnityEvent OnClickEvent => m_OnClick;

    private void Awake()
    {
        m_StartColor = m_BGImage.color;
    }

    public void Activate()
    {
        m_CanvasGroup.interactable = true;
        m_CanvasGroup.blocksRaycasts = true;
        m_BGImage.color = m_StartColor;
    }
    
    public void DeActivate()
    {
        m_CanvasGroup.interactable = false;
        m_CanvasGroup.blocksRaycasts = false;
        m_BGImage.color = Color.Lerp(m_StartColor, Color.black, .5f);
    }

    public void OnClick()
    {
        OnClickEvent.Invoke();
    }
}