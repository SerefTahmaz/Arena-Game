using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class cDamageEffector : cDamageEffectorBase
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out IDamagable damagable))
        {
            var collisionPoint = other.ClosestPoint(transform.position);
            DamageIt(damagable, new DamageWrapper()
            {
                amount = 10, 
                pos = collisionPoint, 
                isHeavyDamage = false,
                damager = transform,
                Instigator = Character
            });
        }
    }
}