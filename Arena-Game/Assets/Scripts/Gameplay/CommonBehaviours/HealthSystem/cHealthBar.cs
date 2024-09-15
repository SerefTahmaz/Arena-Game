using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.UI;
using DG.Tweening;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

public class cHealthBar : MonoBehaviour
{
    [SerializeField] private cView m_View;
    [SerializeField] private cHealthManager m_HealthManager;
    [SerializeField] private ProceduralImage m_Image;
    [SerializeField] private ProceduralImage m_TempImage;
    [SerializeField] private float m_Speed;
    [SerializeField] private TMP_Text m_NameText;

    private bool m_IsDelayCompleted;
    private Tween m_DelayTween;

    private float StartHealth => HealthManager.StartHealth;
    private NetworkVariable<float> CurrentHealth => HealthManager.CurrentHealth;
    private string PlayerName => HealthManager.PlayerName;

    public cHealthManager HealthManager
    {
        get => m_HealthManager;
        set => m_HealthManager = value;
    }

    public Action<bool> m_OnVisibleUpdate = delegate(bool b) {  };

    public void SetVisibility(bool state)
    {
        if (state)
        {
            m_View.Activate();
        }
        else
        {
            m_View.Deactivate();
        }
        
        m_OnVisibleUpdate.Invoke(state);
    }
    
    public void UpdateUI()
    {
        m_NameText.text = PlayerName;
        m_Image.fillAmount = CurrentHealth.Value / StartHealth;
        m_DelayTween.Kill();
        m_DelayTween = DOVirtual.DelayedCall(1, () => m_IsDelayCompleted = true);
        
        Debug.Log($"{CurrentHealth.Value} / {StartHealth}");
    }
    
    private void Update()
    {
        if (m_IsDelayCompleted)
        {
            m_TempImage.fillAmount =
                Mathf.MoveTowards(m_TempImage.fillAmount, m_Image.fillAmount, m_Speed * Time.deltaTime);
        }

        if (Math.Abs(m_TempImage.fillAmount - m_Image.fillAmount) < .001f)
        {
            m_IsDelayCompleted = false;
        }
    }
}
