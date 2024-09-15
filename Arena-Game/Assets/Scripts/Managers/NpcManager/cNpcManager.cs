using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArenaGame.Utils;
using FiniteStateMachine;
using UnityEngine;

public class cNpcManager : cSingleton<cNpcManager>
{
    public List<GameObject> m_Npcs = new List<GameObject>();

    public bool CheckIsAllNpcsDied()
    {
        return m_Npcs.Select((o => o.GetComponentInChildren<cStateMachine>()))
            .All((machine =>
            {
                Debug.Log(machine.CurrentState.GetType());
                return machine.CurrentState.GetType().IsSubclassOf(typeof(cDeath));
            }));
    }

    public void DestroyNpcs()
    {
        foreach (var VARIABLE in m_Npcs)
        {
            Destroy(VARIABLE);
        }
        m_Npcs.Clear();
    }

    public void SetParents()
    {
        
    }
}