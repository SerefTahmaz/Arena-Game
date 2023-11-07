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

public class cHealthManager : MonoBehaviour
{
    [SerializeField] private cCharacter m_Character;
    [SerializeField] private cHealthBar m_WorldHealthBar;
    [SerializeField] private cHealthController m_HealthController;
    [SerializeField] private HealthBarState m_HealthBarState;

    public string PlayerName => m_Character.CharacterName;
    
    public NetworkVariable<float> CurrentHealth => m_Character.CharacterNetworkController.CurrentHealth;

    public bool HasHealth => CurrentHealth.Value > 0;

    public float StartHealth => m_Character.StartHealth;

    public Action m_OnDied = delegate { };

    public cCharacterNetworkController CharacterNetworkController => m_Character.CharacterNetworkController;
    
    private cHealthBar m_HealthBar;
    
    public enum HealthBarState
    {
        World,
        UIBoss,
        UIPlayer
    }

    private void Awake()
    {
        m_Character.CharacterNetworkController.OnSpawn += () =>
        {
            m_HealthBar = m_WorldHealthBar;
            CurrentHealth.OnValueChanged += (value, newValue) => { UpdateUIClientRpc(); };
            if(m_Character.CharacterNetworkController.IsOwner) CurrentHealth.Value = StartHealth;
            m_HealthController.m_OnDied += () =>
            {
                m_OnDied.Invoke();
            };
        };
    }

    public void OnDamage(float damageAmount)
    {
        if (!CharacterNetworkController.IsOwner) return;
        m_HealthController.OnDamage(damageAmount);
    }

    public void UpdateUIClientRpc()
    {
        m_HealthBar.UpdateUI();
    }
    
    private void EnableHealthBar()
    {
        cHealthBar healthBar;
        switch (m_HealthBarState)
        {
            case HealthBarState.World:
                m_HealthBar = m_WorldHealthBar;
                break;
            case HealthBarState.UIBoss:
                healthBar = cGameManager.Instance.GiveMeBossUIHealthBar();
                if (healthBar != null)
                {
                    m_HealthBar = healthBar;
                    m_HealthBar.HealthManager = this;
                }
                break;
            case HealthBarState.UIPlayer:
                healthBar = cGameManager.Instance.GiveMePlayerUIHealthBar();
                if (healthBar != null)
                {
                    m_HealthBar = healthBar;
                    m_HealthBar.HealthManager = this;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        m_HealthBar.SetVisibility(true);
        UpdateUIClientRpc();
    }
    
    private void DisableHealthBar()
    {
        m_HealthBar.SetVisibility(false);
        m_HealthBar = m_WorldHealthBar;
    }

    public void SetVisibility(bool state)
    {
        if (state)
        {
            EnableHealthBar();
        }
        else
        {
            DisableHealthBar();
        }
    }
}