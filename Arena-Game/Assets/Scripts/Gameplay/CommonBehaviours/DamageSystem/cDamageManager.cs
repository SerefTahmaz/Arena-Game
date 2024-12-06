using System;
using System.Collections;
using System.Collections.Generic;
using STNest.Utils;
using UnityEngine;
using UnityEngine.Events;

public class cDamageManager : MonoBehaviour
{
    public cCharacter m_Character;
    public UnityEvent<DamageWrapper> m_OnDamage;

    public List<cDamageReicever> m_DamageHandlers;
    public List<cDamageEffectorBase> m_DamageEffectors;
    
    public bool IsAttacking { get; set; }

    public void Init(int teamId)
    {
        foreach (var VARIABLE in m_DamageHandlers)
        {
            VARIABLE.OnDamage.AddListener(OnDamage);
        }
        UpdateTeamId(teamId);
        
        foreach (var VARIABLE in m_DamageEffectors)
        {
            VARIABLE.Character = m_Character;
            VARIABLE.OnSetActiveDamage += CheckAttacking;
        }
    }

    private void CheckAttacking(bool obj)
    {
        IsAttacking = false;
        foreach (var VARIABLE in m_DamageEffectors)
        {
            if (VARIABLE.IsActive)
            {
                IsAttacking = true;
            }
        }
    }

    public void UpdateTeamId(int teamId)
    {
        foreach (var VARIABLE in m_DamageHandlers)
        {
            VARIABLE.TeamID = teamId;
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
