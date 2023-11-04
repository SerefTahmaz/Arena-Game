using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class cDamageEffectorBase : MonoBehaviour
{
    [SerializeField] private UnityEvent<DamageWrapper> m_OnDamage;

    private int m_TeamId;
    
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

    public void DamageIt(IDamagable damagable, DamageWrapper damageWrapper)
    {
        if(damagable.TeamID != m_TeamId) damagable.Damage(damageWrapper);
    }
}