using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class cDragonDamageEvents : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_DamageEffectors;

    public void SetActiveLeg(int leg)
    {
        m_DamageEffectors[leg].SetActive(true);
    }
    
    public void SetDeActiveLeg(int leg)
    {
        m_DamageEffectors[leg].SetActive(false);
    }

    public void MeleeAttack2Start()
    {
        m_DamageEffectors[0].SetActive(true);
        m_DamageEffectors[1].SetActive(true);
    }
    
    public void MeleeAttack2End()
    {
        m_DamageEffectors[0].SetActive(false);
        m_DamageEffectors[1].SetActive(false);
    }
    
    public void ForwardJumpStart()
    {
        for (int i = 0; i < m_DamageEffectors.Count; i++)
        {
            m_DamageEffectors[i].SetActive(true);
        }
    }
    
    public void ForwardJumpEnd()
    {
        for (int i = 0; i < m_DamageEffectors.Count; i++)
        {
            m_DamageEffectors[i].SetActive(false);
        }
    }
}
