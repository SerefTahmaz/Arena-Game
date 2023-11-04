using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : cInventoryItem
{
    [SerializeField] private cDamageEvents m_DamageEvents;
    [SerializeField] private GameObject m_Colliders;
    [SerializeField] private bool m_IsRightSword;

    private void Awake()
    {
        if (m_IsRightSword)
        {
            m_DamageEvents.m_OnDamageStart += () =>
            {
                m_Colliders.SetActive(true);
            };
            m_DamageEvents.m_OnDamageEnd += () =>
            {
                m_Colliders.SetActive(false);
            };
        }
        else
        {
            m_DamageEvents.m_OnDamageStartLeft += () =>
            {
                m_Colliders.SetActive(true);
            };
            m_DamageEvents.m_OnDamageEndLeft += () =>
            {
                m_Colliders.SetActive(false);
            };
        }
    }
}
