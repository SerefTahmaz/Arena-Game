using System;
using System.Collections.Generic;
using System.Linq;
using _Main.Scripts.Gameplay;
using DG.Tweening;
using FiniteStateMachine;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Character.NPCHuman
{
    public class NPCHumanFight : Grounded
    {
        [SerializeField] private Vector2 m_CooldownDurationRange;
        
        [SerializeField] private float m_MeleeAttackDistance;
        private Transform m_MovementTransform => StateMachine.Character.MovementTransform;
        
        NPCHumanStateMachine StateMachine => m_StateMachine as NPCHumanStateMachine;

        private AnimationController AnimationController => StateMachine.Character.AnimationController;

        private bool m_IsAttackDelayFinished=true;
        
        public override void Enter()
        {
            base.Enter();
            
            if (m_IsAttackDelayFinished == false)
            {
                DOVirtual.DelayedCall(
                    Random.Range(m_CooldownDurationRange.x, m_CooldownDurationRange.y),
                    (() => m_IsAttackDelayFinished = true));
            }
        }
 
        public override void StateMachineUpdate()
        {
            base.StateMachineUpdate();
            
            if(StateMachine.Target() == null) return;

            Vector3 dir = StateMachine.Target().position - m_MovementTransform.position;
            dir.y = 0;
            Vector3 movementVector = m_MovementTransform.forward;
            movementVector.y = 0;
            var angle = Vector3.SignedAngle(movementVector, dir.normalized, Vector3.up);

            if (Vector3.Distance(m_MovementTransform.position, StateMachine.Target().position) > m_MeleeAttackDistance)
            {
                StateMachine.Character.MovementController.Move(dir);
            }
            else
            {
                StateMachine.Character.MovementController.Move(Vector3.zero);
            }
            
           

            if (m_IsAttackDelayFinished)
            {
                if (!StateMachine.Character.CharacterStateMachine.IsLeftSwordDrawn && !StateMachine.Character.CharacterStateMachine.IsRightSwordDrawn)
                {
                    if(Random.value > 0.5f) StateMachine.Character.CharacterStateMachine.SwitchLeftSword();
                    if(Random.value > 0.5f) StateMachine.Character.CharacterStateMachine.SwitchRightSword();
                }
                else if (Attack(angle))
                {
                    m_IsAttackDelayFinished = false;
                    return;
                }
            }

            if (Random.Range(0, 1000) < 10)
            {
                if(Random.value > 0.5f) StateMachine.Character.CharacterStateMachine.SwitchLeftSword();
                if(Random.value > 0.5f) StateMachine.Character.CharacterStateMachine.SwitchRightSword();
            }
        }

        private bool Attack(float angle)
        {
            if (Vector3.Distance(m_MovementTransform.position, StateMachine.Target().position) < m_MeleeAttackDistance)
            {
                List<Action> m_MeleeActions = new List<Action>();
                
                m_MeleeActions.AddRange(Enumerable.Repeat<Action>(() =>
                {
                    if (StateMachine.Character.CharacterStateMachine.IsRightSwordCharged || StateMachine.Character.CharacterStateMachine.IsLeftSwordCharged)
                    {
                        StateMachine.Character.CharacterStateMachine.Slash();
                    }
                    else
                    {
                        StateMachine.Character.CharacterStateMachine.Charge();
                    }
                    
                    DOVirtual.DelayedCall(0.05f, () =>
                    {
                        m_IsAttackDelayFinished = true;
                    });
                }, 5));
            
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