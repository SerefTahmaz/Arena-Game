using System;
using System.Collections;
using System.Collections.Generic;
using DemoBlast.UI;
using DemoBlast.Utils;
using DG.Tweening;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

public class cHealthBar : MonoBehaviour
{
    [SerializeField] private ProceduralImage m_Image;
    [SerializeField] private ProceduralImage m_TempImage;
    [SerializeField] private float m_StartHealth;
    [SerializeField] private float m_Speed;
    [SerializeField] private cView m_View;
    [SerializeField] private cCharacter m_Character;
    [SerializeField] private TMP_Text m_NameText;

    private bool m_IsDelayCompleted;
    private Tween m_DelayTween;

    private NetworkVariable<float> CurrentHealth => m_Character.CharacterNetworkController.CurrentHealth;

    public bool HasHealth => CurrentHealth.Value > 0;
    public Action m_OnDied = delegate { };

    private void Awake()
    {
        CurrentHealth.OnValueChanged += (value, newValue) => { UpdateUIClientRpc(); };
    }

    public void InitHealthBar(string characterName, int startHealth)
    {
        UpdateName(characterName);
        m_StartHealth = startHealth;
        CurrentHealth.Value = m_StartHealth;
    }

    public void SetVisibilty(bool state)
    {
        if (state)
        {
            m_View.Activate();
        }
        else
        {
            m_View.Deactivate();
        }
    }

    public void OnDamage(float damageAmount)
    {
        OnDamageServerRpc(damageAmount);
    }

    public void OnDamageServerRpc(float damageAmount)
    {
        if (!m_Character.CharacterNetworkController.IsOwner) return;

        if (HasHealth == false) return;

        CurrentHealth.Value -= damageAmount;

        if (HasHealth == false)
        {
            m_OnDied.Invoke();
        }
    }

    public void UpdateUIClientRpc()
    {
        m_Image.fillAmount = CurrentHealth.Value / m_StartHealth;
        m_DelayTween.Kill();
        m_DelayTween = DOVirtual.DelayedCall(1, () => m_IsDelayCompleted = true);
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

    public void UpdateName(string newValue)
    {
        m_NameText.text = newValue;
    }
}