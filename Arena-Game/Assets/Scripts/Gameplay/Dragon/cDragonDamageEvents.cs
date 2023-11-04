using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class cDragonDamageEvents : MonoBehaviour
{
    [SerializeField] private List<Transform> m_Transforms;
    [SerializeField] private LayerMask m_LayerMask;

    private List<bool> m_LegActivation = new List<bool>() { false, false, false, false };

    public void SetActiveLeg(int leg)
    {
        m_LegActivation[leg] = true;
    }
    
    public void SetDeActiveLeg(int leg)
    {
        m_LegActivation[leg] = false;
    }

    public void MeleeAttack2Start()
    {
        m_LegActivation[0] = true;
        m_LegActivation[1] = true;
    }
    
    public void MeleeAttack2End()
    {
        m_LegActivation[0] = false;
        m_LegActivation[1] = false;
    }
    
    public void ForwardJumpStart()
    {
        for (int i = 0; i < m_LegActivation.Count; i++)
        {
            m_LegActivation[i] = true;
        }
    }
    
    public void ForwardJumpEnd()
    {
        for (int i = 0; i < m_LegActivation.Count; i++)
        {
            m_LegActivation[i] = false;
        }
    }

    private void Update()
    {
        for (int i = 0; i < m_LegActivation.Count; i++)
        {
            if(m_LegActivation[i] == false) continue;

            var colliders = Physics.OverlapSphere(m_Transforms[i].position, 2, m_LayerMask);
            if (colliders.Any())
            {
                foreach (var VARIABLE in colliders)
                {
                    if (VARIABLE.attachedRigidbody &&VARIABLE.attachedRigidbody.TryGetComponent(out IDamagable damagable))
                    {
                        damagable.Damage(new DamageWrapper()
                        {
                            amount = 20, 
                            pos = Vector3.zero, 
                            isHeavyDamage = true,
                            damager = transform
                        });
                    }
                }
            }
        }
    }
}
