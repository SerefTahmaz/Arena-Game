using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class cDamageManager : MonoBehaviour
{
    public UnityEvent<DamageWrapper> m_OnDamage;

    public List<cDamageHandler> m_DamageHandlers;

    public void Init(int teamId)
    {
        foreach (var VARIABLE in m_DamageHandlers)
        {
            VARIABLE.TeamID = teamId;
            VARIABLE.OnDamage.AddListener(OnDamage);
        }
    }

    public void OnDamage(DamageWrapper damageWrapper)
    {
        m_OnDamage.Invoke(damageWrapper);
    }
}
