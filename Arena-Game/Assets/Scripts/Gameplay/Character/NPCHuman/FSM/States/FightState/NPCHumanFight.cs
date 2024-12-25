using System;
using System.Collections.Generic;
using System.Linq;
using _Main.Scripts.Gameplay;
using ArenaGame.Utils;
using Cysharp.Threading.Tasks;
using DefaultNamespace.FightAI;
using DG.Tweening;
using FiniteStateMachine;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Character.NPCHuman
{
    public class NPCHumanFight : Grounded
    {
        [SerializeField] private FightAISettings m_FightAISettings;
        
        [SerializeField] private float m_MeleeAttackDistance;
        private Transform m_MovementTransform => StateMachine.Character.MovementTransform;
        
        NPCHumanStateMachine StateMachine => m_StateMachine as NPCHumanStateMachine;

        private AnimationController AnimationController => StateMachine.Character.AnimationController;

        public FightAISettings FightAISettings
        {
            get => m_FightAISettings;
            set => m_FightAISettings = value;
        }

        private bool m_IsAttackDelayFinished=true;
        private bool m_IsBuffDecided;
        
        public override void Enter()
        {
            base.Enter();
            if (Random.Range(0, 100) > 15)
            {
                StateMachine.Character.CharacterStateMachine.SwitchRightSword();
            }
            else
            {
                StateMachine.Character.CharacterStateMachine.SwitchLeftSword();
            }

            m_IsBuffDecided = false;
            if(FightAISettings.BuffWithDistanceChance > Random.Range(0,100))
            {
                m_IsBuffDecided = true;
            }
        }
 
        public override void StateMachineUpdate()
        {
            base.StateMachineUpdate();

            if (StateMachine.Target() == null)
            {
                StateMachine.Character.MovementController.Move(Vector3.zero);
                return;
            }

            if (FightAISettings.AvoidAttackChance > Random.Range(0, 100))
            {
                AvoidTargetAttackWithJump();
            }

            
            if (m_IsAttackDelayFinished)
            {
                MoveToTargetWhenDistance();

                if(m_IsBuffDecided)
                {
                    BuffWithDistance();
                }
            }
           
            //
            if (m_IsAttackDelayFinished)
            {
                Attack(); 
            }

            // if (Random.Range(0, 10000) < 10)
            // {
            //     if(Random.value > 0.5f) StateMachine.Character.CharacterStateMachine.SwitchLeftSword();
            //     if(Random.value > 0.5f) StateMachine.Character.CharacterStateMachine.SwitchRightSword();
            // }
        }

        private void MoveToTargetWhenDistance()
        {
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
        }

        private bool Attack()
        {
            if (Vector3.Distance(m_MovementTransform.position, StateMachine.Target().position) < m_MeleeAttackDistance)
            {
                List<Action> m_MeleeActions = new List<Action>();
                
                m_MeleeActions.AddRange(Enumerable.Repeat<Action>(() =>
                {
                    SlashSequence();
                }, 5));

                if (FightAISettings.JumpChance > Random.Range(0, 100))
                {
                    m_MeleeActions.AddRange(Enumerable.Repeat<Action>(() =>
                    {
                        JumpSequence();
                    }, 1));
                }
            
                if (m_MeleeActions.Any())
                {
                    m_MeleeActions.OrderBy((action => Random.Range(0, 10000))).FirstOrDefault().Invoke();
                    return true;
                }
            }

            return false;
        } 

        private void AvoidTargetAttackWithJump()
        {
            if (Vector3.Distance(m_MovementTransform.position, StateMachine.Target().position) > m_MeleeAttackDistance*2) return;
            
            if (StateMachine.Target().parent.parent.TryGetComponent(out cCharacter cCharacter))
            {
                if (cCharacter.DamageManager.IsAttacking)
                {
                    DirectJumpSequence();
                }
            }
        }

        private async UniTask SlashSequence()
        {
            m_IsAttackDelayFinished = false;
            
            Vector3 dir = StateMachine.Target().position - m_MovementTransform.position;
            dir.y = 0;
            Vector3 movementVector = m_MovementTransform.forward;
            movementVector.y = 0;
            var angle = Vector3.SignedAngle(movementVector, dir.normalized, Vector3.up);
            
            var targetAngle = (new List<int>() {5,10, 20, 30, 40, 50 }).RandomItem();
            
            float duration = Random.Range(0.0f, 0.2f);
            while (duration>0 && angle > targetAngle)
            {
                if( StateMachine.Target() == null) return;
                
                dir = StateMachine.Target().position - m_MovementTransform.position;
                dir.y = 0;
                movementVector = m_MovementTransform.forward;
                movementVector.y = 0;
                angle = Vector3.SignedAngle(movementVector, dir.normalized, Vector3.up);
                
                StateMachine.Character.MovementController.Move(dir.normalized/10);

                duration -= Time.deltaTime;
                await UniTask.DelayFrame(1);
            }
             
            StateMachine.Character.MovementController.Move(Vector3.zero, forceValue:true);
            
            StateMachine.Character.CharacterStateMachine.Slash();
                    
            DOVirtual.DelayedCall(Random.Range(FightAISettings.AttackDelayRange.x, FightAISettings.AttackDelayRange.y), () =>
            {
                Debug.Log("Finished attack delay");
                m_IsAttackDelayFinished = true;
            });
        }

        private async UniTask JumpSequence()
        {
            m_IsAttackDelayFinished = false;
            
            Vector3 dir = StateMachine.Target().position - m_MovementTransform.position;
            dir.y = 0;
            Vector3 movementVector = m_MovementTransform.forward;
            movementVector.y = 0;
            var angle = Vector3.SignedAngle(movementVector, dir.normalized, Vector3.up);

            var targetAngle = (new List<int>() { 45, 90, 30, 60 }).RandomItem();
            
            float duration = 0.2f;
            while (duration>0 && angle < targetAngle)
            {
                if (StateMachine.Target() == null)
                {
                    m_IsAttackDelayFinished = true;
                    return;
                }
                
                dir = StateMachine.Target().position - m_MovementTransform.position;
                dir.y = 0;
                movementVector = m_MovementTransform.forward;
                movementVector.y = 0;
                angle = Vector3.SignedAngle(movementVector, dir.normalized, Vector3.up);
                StateMachine.Character.MovementController.Move(-dir.normalized/10);

                duration -= Time.deltaTime;
                await UniTask.DelayFrame(1);
            }
            
            StateMachine.Character.CharacterStateMachine.Jump();

            float durationFocusChar = 0.7f;
            while (durationFocusChar>0)
            {
                if (StateMachine.Target() == null)
                {
                    m_IsAttackDelayFinished = true;
                    return;
                }
                durationFocusChar -= Time.deltaTime;
                MoveToTargetWhenDistance();
            }
                    
            m_IsAttackDelayFinished = true;
        }

        private async UniTask BuffWithDistance()
        {
            if(StateMachine.Character.CharacterStateMachine.IsLeftSwordCharged || StateMachine.Character.CharacterStateMachine.IsRightSwordCharged) return;
            
            m_IsAttackDelayFinished = false;
            while (Vector3.Distance(m_MovementTransform.position, StateMachine.Target().position) < 10)
            {
                await DirectJumpSequence();
            }

            if (!StateMachine.Character.CharacterStateMachine.IsLeftSwordDrawn &&
                !StateMachine.Character.CharacterStateMachine.IsRightSwordDrawn)
            {
                StateMachine.Character.CharacterStateMachine.SwitchRightSword();
            }

            await UniTask.WaitWhile((() => !StateMachine.Character.CharacterStateMachine.IsLeftSwordDrawn &&
                                           !StateMachine.Character.CharacterStateMachine.IsRightSwordDrawn));
            
            StateMachine.Character.CharacterStateMachine.Charge();
            DOVirtual.DelayedCall(1, () =>
            {
                m_IsAttackDelayFinished = true;
            });

        }
        
        private async UniTask DirectJumpSequence()
        {
            m_IsAttackDelayFinished = false;
            
            StateMachine.Character.CharacterStateMachine.Jump();
            
            Vector3 dir = StateMachine.Target().position - m_MovementTransform.position;
            dir.y = 0;
            Vector3 movementVector = m_MovementTransform.forward;
            movementVector.y = 0;

            float duration = 0.7f;
            while (duration>0)
            {
                if (StateMachine.Target() == null)
                {
                    m_IsAttackDelayFinished = true;
                    return;
                }
                
                dir = StateMachine.Target().position - m_MovementTransform.position;
                dir.y = 0;
                movementVector = m_MovementTransform.forward;
                movementVector.y = 0;
                StateMachine.Character.MovementController.Move(-dir.normalized/10);

                duration -= Time.deltaTime;
                await UniTask.DelayFrame(1);
            }
                    
            DOVirtual.DelayedCall(.7f, () =>
            {
                m_IsAttackDelayFinished = true;
            });
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