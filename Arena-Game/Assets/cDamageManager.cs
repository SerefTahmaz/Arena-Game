using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class cDamageManager : MonoBehaviour
{
    public UnityEvent<DamageWrapper> m_OnDamage;

    public List<cDamageReicever> m_DamageHandlers;
    public List<cDamageEffectorBase> m_DamageEffectors;

    public void Init(int teamId)
    {
        foreach (var VARIABLE in m_DamageHandlers)
        {
            VARIABLE.TeamID = teamId;
            VARIABLE.OnDamage.AddListener(OnDamage);
        }
        foreach (var VARIABLE in m_DamageEffectors)
        {
            VARIABLE.TeamID = teamId;
        }
    }

    public void OnDamage(DamageWrapper damageWrapper)
    {
        m_OnDamage.Invoke(damageWrapper);
    }
}
