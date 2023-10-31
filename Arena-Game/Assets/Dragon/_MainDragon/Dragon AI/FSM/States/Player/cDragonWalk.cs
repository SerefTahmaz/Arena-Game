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
    public class cDragonWalk : Grounded
    {
        [SerializeField] private float m_MeleeAttackDistance;
        [SerializeField] private float m_FireBreathingDistance;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RotationSpeed;

        private Transform m_MovementTransform => StateMachine.Character.MovementTransform;
        
        cDragonStateMachine StateMachine => m_StateMachine as cDragonStateMachine;

        private cAnimationController AnimationController => StateMachine.Character.AnimationController;

        private bool m_IsAttackDelayFinished=true;
        
        public override void Enter()
        {
            base.Enter();
            AnimationController.SetTrigger(cAnimationController.eAnimationType.Walk);

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

            if (m_IsAttackDelayFinished)
            {
                if (StateMachine.ZKeyPressed)
                {
                    StateMachine.ZKeyPressed = false;
                    
                    AnimationController.SetTrigger(cAnimationController.eAttackType.FlyBreathing);
                    StateMachine.ChangeState(StateMachine.m_DragonEmpty);
                    m_IsAttackDelayFinished = false;
                    return;
                }
                if (StateMachine.XKeyPressed)
                {
                    StateMachine.XKeyPressed = false;
                    
                    StateMachine.ChangeState(StateMachine.m_DragonFly);
                    m_IsAttackDelayFinished = false;
                    return;
                }
                if (StateMachine.VKeyPressed)
                {
                    StateMachine.VKeyPressed = false;
                    
                    AnimationController.SetTrigger(cAnimationController.eAttackType.MeleeAttack);
                    StateMachine.ChangeState(StateMachine.m_DragonEmpty);
                    m_IsAttackDelayFinished = false;
                    return;
                }
            }

            Vector3 dir = StateMachine.Target.position - m_MovementTransform.position;
            var angle = Vector3.SignedAngle(m_MovementTransform.forward, dir.normalized, Vector3.up);
            m_MovementTransform.Translate(Vector3.forward * Time.deltaTime * m_WalkSpeed);
            var planeDir = dir.normalized;
            planeDir.y = 0;
            var lookRot = Quaternion.LookRotation(planeDir.normalized);
            m_MovementTransform.rotation = Quaternion.Slerp(m_MovementTransform.rotation, lookRot, Time.deltaTime * m_RotationSpeed);

            if (m_IsAttackDelayFinished)
            {
                if (Attack(angle))
                {
                    m_IsAttackDelayFinished = false;
                    return;
                }
            }

           
            
            // Debug.Log(angle);

            if (StateMachine.AvailableAttacks.HasFlag(cAnimationController.eAttackType.Turn360))
            {
                if (Mathf.Abs(angle) > 100)
                {
                    Turn360();
                    StateMachine.ChangeState(StateMachine.m_DragonEmpty);
                    return;
                }
            }
            
            if (angle > 30)
            {
                RightTurn();
                StateMachine.ChangeState(StateMachine.m_DragonEmpty);
                return;
            }
            
            if (angle < -30)
            {
                LeftTurn();
                StateMachine.ChangeState(StateMachine.m_DragonEmpty);
                return;
            }
        }

        private bool Attack(float angle)
        {
            if (Vector3.Distance(m_MovementTransform.position, StateMachine.Target.position) < m_MeleeAttackDistance)
            {
                List<Action> m_MeleeActions = new List<Action>();

                if (StateMachine.AvailableAttacks.HasFlag(cAnimationController.eAttackType.TransitionToFly))
                {
                    m_MeleeActions.AddRange(Enumerable.Repeat<Action>(() => StateMachine.ChangeState(StateMachine.m_DragonFly),
                        5));
                }


                if (StateMachine.AvailableAttacks.HasFlag(cAnimationController.eAttackType.FlyBreathing))
                {
                    m_MeleeActions.AddRange(Enumerable.Repeat<Action>(() =>
                    {
                        AnimationController.SetTrigger(cAnimationController.eAttackType.FlyBreathing);
                        StateMachine.ChangeState(StateMachine.m_DragonEmpty);
                    }, 5));
                }

                if (StateMachine.AvailableAttacks.HasFlag(cAnimationController.eAttackType.ForwardBashBreathingAttack))
                {
                    m_MeleeActions.AddRange(Enumerable.Repeat<Action>(() =>
                    {
                        AnimationController.SetTrigger(cAnimationController.eAttackType.ForwardBashBreathingAttack);
                        StateMachine.ChangeState(StateMachine.m_DragonEmpty);
                    }, 5));
                }

                if (StateMachine.AvailableAttacks.HasFlag(cAnimationController.eAttackType.MeleeAttack))
                {
                    m_MeleeActions.AddRange(Enumerable.Repeat<Action>(() =>
                    {
                        AnimationController.SetTrigger(cAnimationController.eAttackType.MeleeAttack);
                        StateMachine.ChangeState(StateMachine.m_DragonEmpty);
                    }, 5));
                }
                
                if (StateMachine.AvailableAttacks.HasFlag(cAnimationController.eAttackType.ForwardJump))
                {
                    m_MeleeActions.AddRange(Enumerable.Repeat<Action>(() =>
                    {
                        AnimationController.SetTrigger(cAnimationController.eAttackType.ForwardJump);
                        StateMachine.ChangeState(StateMachine.m_DragonEmpty);
                    }, 5));
                }

                if (m_MeleeActions.Any())
                {
                    m_MeleeActions.OrderBy((action => Random.Range(0, 10000))).FirstOrDefault().Invoke();
                    return true;
                }
            }

            if (Vector3.Distance(m_MovementTransform.position, StateMachine.Target.position) < m_FireBreathingDistance &&
                Mathf.Abs(angle) < 30)
            {
                List<Action> m_RangedActions = new List<Action>();

                if (StateMachine.AvailableAttacks.HasFlag(cAnimationController.eAttackType.FireBreathing))
                {
                    m_RangedActions.AddRange(Enumerable.Repeat<Action>(() =>
                    {
                        AnimationController.SetTrigger(cAnimationController.eAttackType.FireBreathing);
                        StateMachine.ChangeState(StateMachine.m_DragonEmpty);
                    }, 10));
                }

                if (StateMachine.AvailableAttacks.HasFlag(cAnimationController.eAttackType.ForwardBash))
                {
                    m_RangedActions.AddRange(Enumerable.Repeat<Action>(() =>
                    {
                        AnimationController.SetTrigger(cAnimationController.eAttackType.ForwardBash);
                        StateMachine.ChangeState(StateMachine.m_DragonEmpty);
                    }, 10));
                }

                if (m_RangedActions.Any())
                {
                    m_RangedActions.OrderBy((action => Random.Range(0, 10000))).FirstOrDefault().Invoke();
                    return true;
                }
            }

            return false;
        }

        public void LeftTurn()
        {
            AnimationController.SetTrigger(cAnimationController.eAnimationType.LeftTurn);
        }
    
        public void RightTurn()
        {
            AnimationController.SetTrigger(cAnimationController.eAnimationType.RightTurn);
        }
        private void Turn360()
        {
            AnimationController.SetTrigger(cAnimationController.eAttackType.Turn360);
        }

        public override void Exit()
        {
            base.Exit();
        }

        private void OnDrawGizmosSelected()
        {
            var color = Gizmos.color;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, m_FireBreathingDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_MeleeAttackDistance);
            Gizmos.color = color;
        }
    }
}