using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : cInventoryItem
{
    [SerializeField] private cDamageEvents m_DamageEvents;
    [SerializeField] private bool m_IsRightSword;
    [SerializeField] private cDamageEffector m_DamageEffector;

    private void Awake()
    {
        if (m_IsRightSword)
        {
            m_DamageEvents.m_OnDamageStart += () =>
            {
                m_DamageEffector.SetActiveDamage(true);
            };
            m_DamageEvents.m_OnDamageEnd += () =>
            {
                m_DamageEffector.SetActiveDamage(false);
            };
        }
        else
        {
            m_DamageEvents.m_OnDamageStartLeft += () =>
            {
                m_DamageEffector.SetActiveDamage(true);
            };
            m_DamageEvents.m_OnDamageEndLeft += () =>
            {
                m_DamageEffector.SetActiveDamage(false);
            };
        }
    }
}
