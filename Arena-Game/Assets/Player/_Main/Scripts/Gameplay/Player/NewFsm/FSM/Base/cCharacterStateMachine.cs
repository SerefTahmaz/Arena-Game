using System.Linq;
using FiniteStateMachine;
using UnityEngine;

public abstract class cCharacterStateMachine:cStateMachine
{
    private Transform m_CurrentTarget;
    public Transform Target()
    {
        if (m_CurrentTarget!= null&&m_enemies.Contains(m_CurrentTarget) && Random.value>.05f)
        {
            return m_CurrentTarget;
        }
        else
        {
            m_CurrentTarget=m_enemies.OrderBy((v2 => Vector3.Distance(transform.position, v2.position))).FirstOrDefault();
            return m_CurrentTarget;
        }
    }
}