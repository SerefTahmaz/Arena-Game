using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace FiniteStateMachine
{
    public class cTrollWalk : Grounded
    {
        [SerializeField] private float m_MeleeAttackDistance;
        private Transform m_MovementTransform => StateMachine.TrollCharacter.MovementTransform;
        
        cTrollStateMachine StateMachine => m_StateMachine as cTrollStateMachine;

        private cTrollAnimationController AnimationController => StateMachine.TrollCharacter.AnimationController;

        private bool m_IsAttackDelayFinished=true;
        
        public override void Enter()
        {
            base.Enter();
            AnimationController.SetTrigger(cTrollAnimationController.TrollAnimationState.Walk);

            if (m_IsAttackDelayFinished == false)
            {
                DOVirtual.DelayedCall(
                    Random.Range(StateMachine.CooldownDurationRange.x, StateMachine.CooldownDurationRange.y),
                    (() => m_IsAttackDelayFinished = true));
            }
        }

        public override void StateMachineUpdate()
        {
            base.StateMachineUpdate();

            if (StateMachine.Target() == null)
            {
                StateMachine.TrollCharacter.MovementController.Move(Vector3.zero);
                return;
            }

            Vector3 dir = StateMachine.Target().position - m_MovementTransform.position;
            dir.y = 0;
            Vector3 movementVector = m_MovementTransform.forward;
            movementVector.y = 0;
            var angle = Vector3.SignedAngle(movementVector, dir.normalized, Vector3.up);

            if (Vector3.Distance(m_MovementTransform.position, StateMachine.Target().position) > m_MeleeAttackDistance)
            {
                StateMachine.TrollCharacter.MovementController.Move(dir);
            }
            else
            {
                StateMachine.TrollCharacter.MovementController.Move(Vector3.zero);
            }

            if (m_IsAttackDelayFinished)
            {
                if (Attack(angle))
                {
                    m_IsAttackDelayFinished = false;
                    return;
                }
            }
        }

        private bool Attack(float angle)
        {
            if (Vector3.Distance(m_MovementTransform.position, StateMachine.Target().position) < m_MeleeAttackDistance)
            {
                List<Action> m_MeleeActions = new List<Action>();


                if (StateMachine.AvailableAttacks.HasFlag(cTrollAnimationController.TrollAnimationState.Attack1))
                {
                    m_MeleeActions.AddRange(Enumerable.Repeat<Action>(() =>
                    {
                        AnimationController.SetTrigger(cTrollAnimationController.TrollAnimationState.Attack1);
                        StateMachine.ChangeState(StateMachine.m_Empty);
                    }, 5));
                }
                
                if (StateMachine.AvailableAttacks.HasFlag(cTrollAnimationController.TrollAnimationState.Attack2))
                {
                    m_MeleeActions.AddRange(Enumerable.Repeat<Action>(() =>
                    {
                        AnimationController.SetTrigger(cTrollAnimationController.TrollAnimationState.Attack2);
                        StateMachine.ChangeState(StateMachine.m_Empty);
                    }, 5));
                }
                
                if (StateMachine.AvailableAttacks.HasFlag(cTrollAnimationController.TrollAnimationState.Attack3))
                {
                    m_MeleeActions.AddRange(Enumerable.Repeat<Action>(() =>
                    {
                        AnimationController.SetTrigger(cTrollAnimationController.TrollAnimationState.Attack3);
                        StateMachine.ChangeState(StateMachine.m_Empty);
                    }, 5));
                }
                
                if (StateMachine.AvailableAttacks.HasFlag(cTrollAnimationController.TrollAnimationState.Attack4))
                {
                    m_MeleeActions.AddRange(Enumerable.Repeat<Action>(() =>
                    {
                        AnimationController.SetTrigger(cTrollAnimationController.TrollAnimationState.Attack4);
                        StateMachine.ChangeState(StateMachine.m_Empty);
                    }, 5));
                }
                
                if (StateMachine.AvailableAttacks.HasFlag(cTrollAnimationController.TrollAnimationState.Taunt))
                {
                    m_MeleeActions.AddRange(Enumerable.Repeat<Action>(() =>
                    {
                        AnimationController.SetTrigger(cTrollAnimationController.TrollAnimationState.Taunt);
                        StateMachine.ChangeState(StateMachine.m_Empty);
                    }, 5));
                }

                if (m_MeleeActions.Any())
                {
                    m_MeleeActions.OrderBy((action => Random.Range(0, 10000))).FirstOrDefault().Invoke();
                    return true;
                }
            }

            return false;
        }

        public override void Exit()
        {
            base.Exit();
        }

        private void OnDrawGizmosSelected()
        {
            var color = Gizmos.color;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_MeleeAttackDistance);
            Gizmos.color = color;
        }
    }
}