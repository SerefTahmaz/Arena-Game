using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FiniteStateMachine;
using STNest.Utils;
using UnityEngine;

namespace ArenaGame
{
    public class NPCTargetHelper
    {
        private cStateMachine m_StateMachine;

        private ObservableList<cCharacter> Enemies => m_StateMachine.m_enemies;
        
        public bool IsAggressive { get; set; }

        public NPCTargetHelper(cStateMachine stateMachine)
        {
            m_StateMachine = stateMachine;
            stateMachine.m_enemies.Updated += EnemiesOnUpdated;
        }

        private void EnemiesOnUpdated()
        {
            Debug.Log($"Enemies Updated");
            if (Enemies.Count > 0)
            {
                m_FocusDuration = 2;
                m_CurrentTarget = Enemies.Last();
            }
        }

        private cCharacter m_CurrentTarget;
        private float m_FocusDuration;
        
        public Transform Target()
        {
            if (Enemies.Count <= 0)
            {
                if (IsAggressive)
                {
                    var player = GameObject.FindObjectsOfType<cPlayerStateMachineV2>().Where((v2 => v2.Character.HealthManager.HasHealth))
                        .OrderBy((v2 => Vector3.Distance(m_StateMachine.transform.position, v2.Character.MovementTransform.position))).FirstOrDefault();
                    if (player)
                    {
                        Enemies.Add(player.Character);
                    }
                }
                return null;
            }

            if (Enemies.Where((character => !character.HealthManager.HasHealth)).Any())
            {
                Enemies.RemoveAll((character => !character.HealthManager.HasHealth));
            }
            if (!Enemies.Contains(m_CurrentTarget))
            {
                m_CurrentTarget = null;
            }
        
            if (m_CurrentTarget != null && m_FocusDuration > 0)
            {
                m_FocusDuration -= Time.deltaTime;
                return m_CurrentTarget.MovementTransform;
            }
            else
            {
                // Debug.Log($"Distance {Vector3.Distance(m_StateMachine.transform.position, m_CurrentTarget.MovementTransform.position) }");
                if (m_CurrentTarget == null || Vector3.Distance(m_StateMachine.transform.position, m_CurrentTarget.MovementTransform.position) > 8)
                {
                    m_CurrentTarget=Enemies.OrderBy((v2 => Vector3.Distance(m_StateMachine.transform.position, v2.MovementTransform.position))).FirstOrDefault();
                }

                if (m_CurrentTarget)
                {
                    return m_CurrentTarget.MovementTransform;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}