using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cFireCollisionHandler : MonoBehaviour
{
    [SerializeField] private cCharacter m_Character;
    
    private void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent(out IDamagable damagable))
        {
            if(damagable.TeamID != m_Character.TeamID) damagable.Damage(10, Vector3.zero, false);
        }
    }
}
