using System;
using System.Collections.Generic;
using System.Linq;
using _Main.Scripts.Gameplay;
using ArenaGame.Utils;
using DefaultNamespace;
using DG.Tweening;
using FiniteStateMachine;
using PlayerCharacter;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Gameplay.Character.NPCHuman
{
    public class PathingNPCPatrol : Grounded
    {
        [SerializeField] private float m_StopDist;
        [SerializeField] private float m_Speed;
        [SerializeField] private float m_PositionSetDuration;
        [SerializeField] private float m_YHelp;
        [SerializeField] private Transform m_TempTarget;

        private float m_CurrentDuration;
        private Transform m_Target;
        
        private PathingNPCStateMachine StateMachine => m_StateMachine as PathingNPCStateMachine;
        private NavMeshAgent Agent => StateMachine.Character.NavMeshAgent;
        private Transform MovementTransform => StateMachine.Character.MovementTransform;
        private MovementController MovementController => StateMachine.Character.MovementController;
        private AgentController AgentController => StateMachine.Character.AgentController;
        
        public override void Enter()
        {
            base.Enter();
            PickATargetPoint();
        }

        private void PickATargetPoint()
        {
            m_Target = StateMachine.PatrolPath.NextPoint(m_Target, MovementTransform);
        }

        public override void StateMachineUpdate()
        {
            base.StateMachineUpdate();

            if (m_TempTarget)
            {
                Agent.SetDestination(m_TempTarget.position);
                var desiredVelocityNormalized = Agent.desiredVelocity.normalized;
                MovementController.Move(desiredVelocityNormalized*m_Speed);
            }
            else
            {
                if (m_Target == null)
                {
                    PickATargetPoint();
                    MovementController.Move(Vector3.zero);
                    return;
                }
            
                var targetPoint = Agent.steeringTarget;
                Vector3 dir = targetPoint - MovementTransform.position;
                // dir.y = 0;
                dir.Normalize();
                // Vector3 movementVector = MovementTransform.forward;
                // movementVector.y = 0;
                // var angle = Vector3.SignedAngle(movementVector, dir, Vector3.up);

                if (Vector3.Distance(MovementTransform.position, m_Target.position) > m_StopDist)
                {
                    Agent.SetDestination(m_Target.position);
                    var desiredVelocityNormalized = Agent.desiredVelocity.normalized;
                    MovementController.Move(desiredVelocityNormalized*m_Speed);

                    // StateMachine.Character.MovementTransform.position += Vector3.up * dir.y * m_YHelp;
                }
                else
                {
                    PickATargetPoint();
                }
            }
           
            if (m_CurrentDuration <= 0)
            {
                m_CurrentDuration = m_PositionSetDuration;
                Agent.nextPosition = transform.position; 
            }

            m_CurrentDuration -= Time.deltaTime;
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}