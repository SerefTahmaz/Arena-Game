using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class cDamageEffector : cDamageEffectorBase
{
    [SerializeField] private List<GameObject> m_Colliders;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out IDamagable damagable))
        {
            var collisionPoint = other.ClosestPoint(transform.position);
            DamageIt(damagable, new DamageWrapper()
            {
                amount = 5, 
                pos = collisionPoint, 
                isHeavyDamage = false,
                damager = transform,
                Instigator = Character
            });
        }
    }

    public override void SetActiveDamage(bool value)
    {
        foreach (var VARIABLE in m_Colliders)
        {
            VARIABLE.SetActive(value);
        }
        base.SetActiveDamage(value);
    }
}