using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cFireCollisionHandler : cDamageEffectorBase
{
    private void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent(out IDamagable damagable))
        {
            DamageIt(damagable, new DamageWrapper()
            {
                amount = 10, 
                pos = Vector3.zero, 
                isHeavyDamage = false,
                damager = transform,
                Instigator = Character
            });
        }
    }
}
