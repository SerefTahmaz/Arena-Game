using System;
using System.Linq;
using DefaultNamespace;
using DG.Tweening;
using FiniteStateMachine;
using PlayerCharacter;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Character.NPCHuman
{
    public class PathingNPCPatrol : Grounded
    {
        [SerializeField] private float m_StopDist;
        [SerializeField] private float m_Speed;
        [SerializeField] private float m_PositionSetDuration; 
        // [SerializeField] private float m_YHelp;
        [SerializeField] private Transform m_TempTarget; 
        [SerializeField] private float m_StopDuration;
        [SerializeField] private float m_GivingWayDuration;
        [SerializeField] private float m_AgentAvoidanceDist;
        [SerializeField] private float m_StopSpeed;
        [SerializeField] private float m_AgentDecisionLerpSpeed;
        [SerializeField] private SplineNavigator m_SplineNavigator;
        
        [SerializeField] private Vector3 m_SpeedDebug;
        [SerializeField] private float m_SpeedMagnitude;

        private float m_StopDurationTimer;
        private bool m_GivingWay;
        private Tween m_GivingWayTween;
        private Vector3 m_MovementVector;

        private float m_CurrentDuration;
        
        private PathingNPCStateMachine StateMachine => m_StateMachine as PathingNPCStateMachine;
        private NavMeshAgent Agent => StateMachine.Character.NavMeshAgent;
        private Transform MovementTransform => StateMachine.Character.MovementTransform;
        private MovementController MovementController => StateMachine.Character.MovementController;
        private AgentController AgentController => StateMachine.Character.AgentController;

        public override void InitializeState(string stateName, cStateMachine playerStateMachine)
        {
            base.InitializeState(stateName, playerStateMachine);
            m_SplineNavigator = new SplineNavigator(StateMachine.PatrolPath, transform.position, m_StopDist);
        }

        public override void Enter()
        {
            base.Enter();
            Agent.SetDestination(m_SplineNavigator.CurrentTargetPos);
        }

        private void PickATargetPoint()
        {
            m_SplineNavigator.PickNextPoint();
            Agent.SetDestination(m_SplineNavigator.CurrentTargetPos);
        }

        public override void StateMachineUpdate()
        {
            base.StateMachineUpdate();
            
            m_SpeedDebug = StateMachine.Character.Rigidbody.velocity;
            m_SpeedMagnitude = StateMachine.Character.Rigidbody.velocity.magnitude;


            if (m_TempTarget)
            {
                Agent.SetDestination(m_TempTarget.position);
                var desiredVelocityNormalized = Agent.desiredVelocity.normalized;
                MovementController.Move(desiredVelocityNormalized*m_Speed);
            }
            else
            {
                // if (m_Target == null)
                // {
                //     PickATargetPoint();
                //     MovementController.Move(Vector3.zero);
                //     return;
                // }
            
                var targetPoint = Agent.steeringTarget;
                Vector3 dir = targetPoint - MovementTransform.position;
                // dir.y = 0;
                dir.Normalize();
                // Vector3 movementVector = MovementTransform.forward;
                // movementVector.y = 0;
                // var angle = Vector3.SignedAngle(movementVector, dir, Vector3.up);

                if (m_SplineNavigator.IsTargetReached(transform.position))
                {
                    PickATargetPoint();
                }
                else
                {
                    if(Agent.enabled) Agent.SetDestination(m_SplineNavigator.CurrentTargetPos); 
                    var desiredVelocityNormalized = Agent.desiredVelocity.normalized;
                    m_MovementVector = Vector3.Slerp(m_MovementVector, desiredVelocityNormalized,
                        Time.deltaTime * m_AgentDecisionLerpSpeed);
                    MovementController.Move(m_MovementVector*m_Speed);
                    // StateMachine.Character.MovementTransform.position += Vector3.up * dir.y * m_YHelp;
                }
            }
           
            if (m_CurrentDuration <= 0)
            {
                m_CurrentDuration = m_PositionSetDuration;
                Agent.nextPosition = transform.position; 
            }
            
            m_CurrentDuration -= Time.deltaTime;

            if (m_SplineNavigator != null)
            {
                if (StateMachine.Character.Rigidbody.velocity.magnitude <= m_StopSpeed)
                {
                    var cols = Physics.OverlapSphere(MovementTransform.position, m_AgentAvoidanceDist);
                    var otherAgents = cols.Where((collider1 =>
                            collider1.attachedRigidbody &&
                            collider1.attachedRigidbody.TryGetComponent(out AgentController agent) && !agent.IsGivingWay
                            && Vector3.Dot(MovementTransform.forward, agent.transform.forward) < -.3f))
                        .Select((collider1 => collider1.attachedRigidbody.GetComponent<AgentController>()))
                        .Except((new []{AgentController}));
            
                    if (otherAgents.Any())
                    {
                        m_StopDurationTimer += Time.deltaTime;
                    }
                    else
                    {
                        m_StopDurationTimer = 0;
                        // if (m_GivingWay)
                        // {
                        //     m_GivingWayTween.Complete(true);
                        // }
                    }
            
                    if (m_StopDurationTimer > m_StopDuration && !m_GivingWay)
                    {
                        m_StopDurationTimer = 0;
                        AgentController.SetObstacle(true);
                        m_GivingWay = true;
                        m_GivingWayTween = DOVirtual.DelayedCall(m_GivingWayDuration, () =>
                        {
                            AgentController.SetObstacle(false);
                            m_GivingWay = false;
                        });
                    }
                }
            }
        }

        public override void Exit()
        {
            Agent.isStopped = true;
            MovementController.Move(Vector3.zero);
            base.Exit();
        }
    }
}