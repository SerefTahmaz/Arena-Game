using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class cDamageReicever : MonoBehaviour,IDamagable
{
    [SerializeField] private UnityEvent<DamageWrapper> m_OnDamage;

    private int m_TeamId;
    
    private bool m_Damaged = false;

    public Transform FocusPoint => transform;

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