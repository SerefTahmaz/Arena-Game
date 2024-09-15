using System.Collections.Generic;
using System.Linq;
using FiniteStateMachine;
using UnityEngine;

namespace ArenaGame
{
    public class NPCTargetHelper
    {
        private cStateMachine m_StateMachine;

        private List<Transform> Enemies => m_StateMachine.m_enemies;

        public NPCTargetHelper(cStateMachine stateMachine)
        {
            m_StateMachine = stateMachine;
        }
        
        private Transform m_CurrentTarget;
        public Transform Target()
        {
            if (Enemies.Count <= 0) return null;
        
            if (m_CurrentTarget != null && Enemies.Contains(m_CurrentTarget) && Random.value>.05f)
            {
                return m_CurrentTarget;
            }
            else
            {
                m_CurrentTarget=Enemies.OrderBy((v2 => Vector3.Distance(m_StateMachine.transform.position, v2.position))).FirstOrDefault();
                return m_CurrentTarget;
            }
        }
    }
}