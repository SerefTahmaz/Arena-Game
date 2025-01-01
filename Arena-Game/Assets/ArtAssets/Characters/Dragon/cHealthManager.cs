using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.UI;
using ArenaGame.Utils;
using DG.Tweening;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

public class cHealthManager : MonoBehaviour
{
    [SerializeField] private cCharacter m_Character;
    [SerializeField] private cHealthBar m_WorldHealthBar;
    [SerializeField] private cHealthController m_HealthController;
    
    private eHealthBarState m_HealthBarState;

    public NetworkVariable<FixedString128Bytes> PlayerName => m_Character.CharacterNetworkController.PlayerName;
    
    public NetworkVariable<float> CurrentHealth => m_Character.CharacterNetworkController.CurrentHealth;

    public bool HasHealth => CurrentHealth.Value > 0;

    public float StartHealth => m_Character.StartHealth;

    public Action m_OnDied = delegate { };

    public cCharacterNetworkController CharacterNetworkController => m_Character.CharacterNetworkController;

    public eHealthBarState HealthBarState
    {
        get => m_HealthBarState;
        set => m_HealthBarState = value;
    }

    private cHealthBar m_HealthBar;

    public enum eHealthBarState
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
            if (m_Character.CharacterNetworkController.IsOwner)
            {
                Debug.Log($"StartHealth Health {StartHealth}");
                CurrentHealth.Value = StartHealth;
            }

            PlayerName.OnValueChanged += (value, newValue) => { UpdateUIClientRpc(); }; 
            
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
        if (cGameManager.Instance == null) HealthBarState = eHealthBarState.World;
        switch (HealthBarState)
        {
            case eHealthBarState.World:
                m_HealthBar = m_WorldHealthBar;
                break;
            case eHealthBarState.UIBoss:
                healthBar = GameHealthBarManager.Instance.GiveMeBossUIHealthBar();
                if (healthBar != null)
                {
                    m_HealthBar = healthBar;
                    m_HealthBar.HealthManager = this;
                }
                break;
            case eHealthBarState.UIPlayer:
                if (CharacterNetworkController.IsOwner)
                {
                    healthBar = GameHealthBarManager.Instance.GiveMePlayerUIHealthBar();
                    if (healthBar != null)
                    {
                        m_HealthBar = healthBar;
                        m_HealthBar.HealthManager = this;
                    }
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        m_HealthBar.SetVisibility(true);
        UpdateUIClientRpc();
    }
    
    public void DisableHealthBar()
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