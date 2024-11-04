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

        private float m_CurrentDuration;
        private Transform m_Target;
        
        private PathingNPCStateMachine StateMachine => m_StateMachine as PathingNPCStateMachine;
        private NavMeshAgent Agent => StateMachine.Character.NavMeshAgent;
        private Transform MovementTransform => StateMachine.Character.MovementTransform;
        private MovementController MovementController => StateMachine.Character.MovementController;
        private AgentController AgentController => StateMachine.Character.AgentController;

        private bool m_IsAttackDelayFinished=true;
        
        public override void Enter()
        {
            base.Enter();
            PickATargetPoint();
        }

        private void PickATargetPoint()
        {
            var targets = PathingHelper.Instance.Paths;
            var suitableTargets = targets.Where((IsSuitable));
            m_Target = suitableTargets
                .OrderBy((transform1 => Vector3.Distance(transform1.position, MovementTransform.position)))
                .FirstOrDefault();
        }

        private bool IsSuitable(Transform transform1)
        {
            var distance = Vector3.Distance(MovementTransform.position, transform1.position);
            var direction = transform1.position - MovementTransform.position;
            direction.Normalize();
            var dot = Vector3.Dot(direction, MovementTransform.forward);
            return dot > -.86f && m_Target != transform1;
        }

        public override void StateMachineUpdate()
        {
            base.StateMachineUpdate();

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
                Debug.Log($"Velocity {desiredVelocityNormalized}");
                Debug.Log($"Steering {dir}");
                MovementController.Move(desiredVelocityNormalized*m_Speed);

                // StateMachine.Character.MovementTransform.position += Vector3.up * dir.y * m_YHelp;
            }
            else
            {
                PickATargetPoint();
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