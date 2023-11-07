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

public class cHealthController : MonoBehaviour
{
    [SerializeField] private cHealthManager m_HealthManager;
    
    public NetworkVariable<float> CurrentHealth => m_HealthManager.CurrentHealth;

    public bool HasHealth => CurrentHealth.Value > 0;

    public float StartHealth => m_HealthManager.StartHealth;

    public Action m_OnDied = delegate { };
    
    public Action m_OnHealthUpdate = delegate {  };
    
    public cCharacterNetworkController CharacterNetworkController => m_HealthManager.CharacterNetworkController;

    public void InitHealthBar(cHealthBar healthBar)
    {
        if(CharacterNetworkController.IsOwner) CurrentHealth.Value = StartHealth;
        
        m_OnHealthUpdate.Invoke();
    }

    public void OnDamage(float damageAmount)
    {
        OnDamageServerRpc(damageAmount);
    }

    public void OnDamageServerRpc(float damageAmount)
    {
        if (HasHealth == false) return;

        CurrentHealth.Value -= damageAmount;

        if (HasHealth == false)
        {
            m_OnDied.Invoke();
        }
    }
}