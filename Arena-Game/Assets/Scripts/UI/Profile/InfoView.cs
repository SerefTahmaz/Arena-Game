using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame;
using ArenaGame.Currency;
using ArenaGame.Managers.SaveManager;
using DG.Tweening;
using TMPro;
using UnityEngine;

public abstract class InfoView : MonoBehaviour
{
    [SerializeField] private TMP_Text m_Text;

    private Tween m_LastTween;
    private int m_LastValue;

    public abstract int TargetValue { get; }
    
    // Start is called before the first frame update
    void Start()
    {
        m_LastValue = TargetValue;
        m_Text.text = m_LastValue.ToString();
        
        SaveGameHandler.OnChanged += HandleOnChange;
    }

    private void HandleOnChange()
    {
        var currentCurrency =  TargetValue;
        
        if(m_LastValue == currentCurrency) return;
        
        m_LastTween.Complete(true);
        m_Text.color = Color.green;
        var startValue = m_LastValue;
        m_LastValue = currentCurrency;
        m_LastTween = DOVirtual.Float(0, 1, 3, value =>
        {
            m_Text.text = (Mathf.FloorToInt(Mathf.Lerp(startValue, m_LastValue, value))).ToString();
        }).OnComplete((() =>
        {
            m_Text.color = Color.white;
        }));
    }
}