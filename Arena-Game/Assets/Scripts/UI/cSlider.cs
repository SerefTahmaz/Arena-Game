using System;
using ArenaGame.Utils;
using AudioSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class cSlider : MonoBehaviour
{
    [SerializeField] private CanvasGroup m_CanvasGroup;
    [SerializeField] private Slider m_Slider;
    [SerializeField] private cCellEventHandler m_CellEventHandler;
    [SerializeField] private UnityEvent<float> m_OnValueChanged;
    [SerializeField] private UnityEvent<float> m_OnValueChangedEnded;
    
    public UnityEvent<float> OnValueChangedEvent => m_OnValueChanged;
    public UnityEvent<float> OnValueChangedEndedEvent => m_OnValueChangedEnded;

    private void Awake()
    {
        m_Slider.onValueChanged.AddListener(OnValueChanged);
        m_CellEventHandler.OnUp.AddListener(OnPointerUp);
    }

    private void OnPointerUp()
    {
        OnValueChangedEndedEvent.Invoke(m_Slider.value);
    }

    public void Activate()
    {
        m_CanvasGroup.interactable = true;
        m_CanvasGroup.blocksRaycasts = true;
    }
    
    public void DeActivate()
    {
        m_CanvasGroup.interactable = false;
        m_CanvasGroup.blocksRaycasts = false;
    }

    public void OnValueChanged(float value)
    {
        OnValueChangedEvent.Invoke(value);
    }

    public void HandleIncreaseClicked()
    {
        m_Slider.value += 0.1f;
    }
    
    public void HandleDecreaseClicked()
    {
        m_Slider.value -= 0.1f;
    }

    public void SetValue(float value)
    {
        m_Slider.value = value;
    }
}