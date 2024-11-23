using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class cDamageReicever : MonoBehaviour,IDamagable
{
    [SerializeField] private UnityEvent<DamageWrapper> m_OnDamage;
    [SerializeField] private Transform m_FocusPoint;
    [SerializeField] private cCharacter m_Character;

    private int m_TeamId;
    
    private bool m_Damaged = false;

    public Transform FocusPoint => m_FocusPoint;
    public bool IsDead => !m_Character.HealthManager.HasHealth;

    public UnityEvent<DamageWrapper> OnDamage
    {
        get => m_OnDamage;
        set => m_OnDamage = value;
    }

    public int TeamID
    {
        get => m_TeamId;
        set => m_TeamId = value;
    }

    public void Damage(DamageWrapper damageWrapper)
    {
        if (m_Damaged == false)
        {
            m_OnDamage.Invoke(damageWrapper);
            
            m_Damaged = true;
            DOVirtual.DelayedCall(.2f, () => m_Damaged = false);
        }
    }
}