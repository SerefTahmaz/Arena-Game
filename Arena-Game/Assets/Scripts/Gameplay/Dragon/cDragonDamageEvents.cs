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
                        damagable.Damage(20,Vector3.zero, true);
                    }
                }
            }
        }
    }
}
