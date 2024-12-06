using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class cDragonDamageEvents : MonoBehaviour
{
    [SerializeField] private List<cDamageEffectorBase> m_DamageEffectors;

    public void SetActiveLeg(int leg)
    {
        m_DamageEffectors[leg].SetActiveDamage(true);
    }
    
    public void SetDeActiveLeg(int leg)
    {
        m_DamageEffectors[leg].SetActiveDamage(false);
    }

    public void MeleeAttack2Start()
    {
        m_DamageEffectors[0].SetActiveDamage(true);
        m_DamageEffectors[1].SetActiveDamage(true);
    }
    
    public void MeleeAttack2End()
    {
        m_DamageEffectors[0].SetActiveDamage(false);
        m_DamageEffectors[1].SetActiveDamage(false);
    }
    
    public void ForwardJumpStart()
    {
        for (int i = 0; i < m_DamageEffectors.Count; i++)
        {
            m_DamageEffectors[i].SetActiveDamage(true);
        }
    }
    
    public void ForwardJumpEnd()
    {
        for (int i = 0; i < m_DamageEffectors.Count; i++)
        {
            m_DamageEffectors[i].SetActiveDamage(false);
        }
    }
}
