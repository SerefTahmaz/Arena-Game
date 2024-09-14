using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Main.Scripts.Gameplay;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace FiniteStateMachine
{
    public class cFightState : Grounded
    {
        cPlayerStateMachineV2 m_PlayerStateMachine => m_StateMachine as cPlayerStateMachineV2;

        public override void Enter()
        {
            base.Enter();
            
            Debug.Log("FightState");
            m_PlayerStateMachine.InputManager.AddListenerToOnRightLightAttackEvent(TryRightAttack);
            m_PlayerStateMachine.InputManager.AddListenerToOnLeftLightAttackEvent(TryLeftAttack);
            m_PlayerStateMachine.InputManager.AddListenerToOnJumpEvent(OnSpace);
            
            m_PlayerStateMachine.InputManager.AddListenerToOnDrawLeftItem(SwitchLeftSword);
            m_PlayerStateMachine.InputManager.AddListenerToOnDrawRightItem(SwitchRightSword);
            
            m_PlayerStateMachine.InputManager.AddListenerToOnEnableLeftHandBuffEvent(TryCharge);
            
            m_PlayerStateMachine.InputManager.AddListenerToOnNum3Event(OnHelloEveryone);
            m_PlayerStateMachine.InputManager.AddListenerToOnNum4Event(OnStretching);
        }

        private void OnStretching()
        {
            m_PlayerStateMachine.Character.CharacterStateMachine.PlayStreach();
        }

        private void OnHelloEveryone()
        {
            m_PlayerStateMachine.Character.CharacterStateMachine.PlayHelloEverybody();
        }
        
        void TryRightAttack()
        {
            m_PlayerStateMachine.Character.CharacterStateMachine.Slash();
        }
        
        void TryLeftAttack()
        {
            m_PlayerStateMachine.Character.CharacterStateMachine.Slash();
        }

        public void TryCharge()
        {
            m_PlayerStateMachine.Character.CharacterStateMachine.Charge();
        }

        public void SwitchLeftSword()
        {
            m_PlayerStateMachine.Character.CharacterStateMachine.SwitchLeftSword();
        }
        
        public void SwitchRightSword()
        {
            m_PlayerStateMachine.Character.CharacterStateMachine.SwitchRightSword();
        }

        void OnSpace()
        {
            m_PlayerStateMachine.Character.CharacterStateMachine.Jump();
        }
        
        public override void StateMachineUpdate()
        {
            // if (m_IsTakingDamage)
            // {
            //     return;
            // }
            base.StateMachineUpdate();
            m_PlayerStateMachine.MovementUserController.Movement();
        }

        public override void OnDied()
        {
            base.OnDied();
            m_PlayerStateMachine.ChangeState(m_PlayerStateMachine.Dead);
        }

        public override void Exit()
        {
            base.Exit();
            m_PlayerStateMachine.InputManager.RemoveListenerToOnRightLightAttackEvent(TryRightAttack);
            m_PlayerStateMachine.InputManager.RemoveListenerToOnLeftLightAttackEvent(TryLeftAttack);
            m_PlayerStateMachine.InputManager.RemoveListenerToOnJumpEvent(OnSpace);
            
            m_PlayerStateMachine.InputManager.RemoveListenerToOnDrawLeftItem(SwitchLeftSword);
            m_PlayerStateMachine.InputManager.RemoveListenerToOnDrawRightItemEvent(SwitchRightSword);
            
            m_PlayerStateMachine.InputManager.RemoveListenerToOnEnableLeftHandBuffEvent(TryCharge);
            
            m_PlayerStateMachine.InputManager.RemoveListenerToOnNum3Event(OnHelloEveryone);
            m_PlayerStateMachine.InputManager.RemoveListenerToOnNum4Event(OnStretching);
        }

        private void OnDestroy()
        {
            try
            {
                m_PlayerStateMachine.InputManager.RemoveListenerToOnRightLightAttackEvent(TryRightAttack);
                m_PlayerStateMachine.InputManager.RemoveListenerToOnLeftLightAttackEvent(TryLeftAttack);
                m_PlayerStateMachine.InputManager.RemoveListenerToOnJumpEvent(OnSpace);
            
                m_PlayerStateMachine.InputManager.RemoveListenerToOnDrawLeftItem(SwitchLeftSword);
                m_PlayerStateMachine.InputManager.RemoveListenerToOnDrawRightItemEvent(SwitchRightSword);
            
                m_PlayerStateMachine.InputManager.RemoveListenerToOnEnableLeftHandBuffEvent(TryCharge);
            
                m_PlayerStateMachine.InputManager.RemoveListenerToOnNum3Event(OnHelloEveryone);
                m_PlayerStateMachine.InputManager.RemoveListenerToOnNum4Event(OnStretching);
            }
            catch (NullReferenceException e)
            {
                Debug.Log("Already Deleted");
            }
        }
    }
}