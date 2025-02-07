using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class cDamageEffectorBase : MonoBehaviour
{
    [SerializeField] protected float m_DamageAmount;
    
    [SerializeField] private UnityEvent<DamageWrapper> m_OnDamage;

    private int m_TeamId;
    private cCharacter m_Character;
    
    private bool m_Damaged = false;

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

    public cCharacter Character
    {
        get => m_Character;
        set => m_Character = value;
    }
    
    public Action<bool> OnSetActiveDamage { get; set; }
    public bool IsActive { get; set; }

    public void DamageIt(IDamagable damagable, DamageWrapper damageWrapper)
    {
        if (damagable.TeamID != m_TeamId)
        {
            damageWrapper.Instigator = Character;
            damagable.Damage(damageWrapper);
        }
    }

    public virtual void SetActiveDamage(bool value)
    {
        IsActive = value;
        OnSetActiveDamage?.Invoke(value);
    }
}