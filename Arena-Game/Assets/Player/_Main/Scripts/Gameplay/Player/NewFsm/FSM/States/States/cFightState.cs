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
    public class cFightState : Grounded
    {
        enum SwordHandness
        {
            Right,
            Left,
            Both
        }
     
        cPlayerStateMachineV2 m_PlayerStateMachine => m_StateMachine as cPlayerStateMachineV2;
        private bool m_IsTakingDamage = false;

        private bool m_IsLeftSwordDrawn = false;
        private bool m_IsRightSwordDrawn = false;

        public override void Enter()
        {
            base.Enter();
            m_PlayerStateMachine.AnimationController.SetTrigger(AnimationController.AnimationState.FightIdle);
            
            Debug.Log("FightState");
            m_PlayerStateMachine.InputManager.AddListenerToOnRightLightAttackEvent(OnClick);
            m_PlayerStateMachine.InputManager.AddListenerToOnUnArmEvent(OnRKey);
            m_PlayerStateMachine.InputManager.AddListenerToOnLeftLightAttackEvent(OnRightClickDown);
            m_PlayerStateMachine.InputManager.AddListenerToOnJumpEvent(OnSpace);
            
            m_PlayerStateMachine.InputManager.AddListenerToOnDrawLeftItem(SwitchLeftSword);
            m_PlayerStateMachine.InputManager.AddListenerToOnDrawRightItem(SwitchRightSword);
            
            m_PlayerStateMachine.InputManager.AddListenerToOnEnableLeftHandBuffEvent(OnChargeLeft);
            m_PlayerStateMachine.InputManager.AddListenerToOnEnableRightHandBuffEvent(OnChargeRight);
            
            m_PlayerStateMachine.InputManager.AddListenerToOnTwoHandedAttackEvent(OnHeavyAttack);
            
            m_PlayerStateMachine.InputManager.AddListenerToOnFKeyDownEvent(OnChargeBoth);
            
            m_PlayerStateMachine.InputManager.AddListenerToOnNum3Event(OnHelloEveryone);
            m_PlayerStateMachine.InputManager.AddListenerToOnNum4Event(OnStretching);
        }

        private void OnStretching()
        {
            m_PlayerStateMachine.AnimationController.SetTrigger(AnimationController.AnimationState.Stretching);
        }

        private void OnHelloEveryone()
        {
            m_PlayerStateMachine.AnimationController.SetTrigger(AnimationController.AnimationState.HelloEverybody);
        }

        private void OnChargeBoth()
        {
            m_PlayerStateMachine.AnimationController.SetTrigger(AnimationController.AnimationState.ChargeBoth, resetable: true);
        }
        
        void OnClick()
        {
            if (m_IsLeftSwordDrawn && m_IsRightSwordDrawn)
            {
                OnHeavyAttack();
            }
            else if (m_IsRightSwordDrawn)
            {
                m_PlayerStateMachine.AnimationController.SetTrigger(AnimationController.AnimationState.Slash, resetable: true);
            }
            else if (m_IsLeftSwordDrawn)
            {
                OnRightClickDown();
            }
        }
        
        void OnRightClickDown()
        {
            m_PlayerStateMachine.AnimationController.SetTrigger(AnimationController.AnimationState.LeftSlash, resetable: true);
        }

        private void OnHeavyAttack()
        {
            m_PlayerStateMachine.AnimationController.SetTrigger(AnimationController.AnimationState.HeavySlash, resetable: true);
        }

        public void OnChargeLeft()
        {
            m_PlayerStateMachine.AnimationController.SetTrigger(AnimationController.AnimationState.ChargeLeft, resetable: true);
        }
        
        public void OnChargeRight()
        {
            m_PlayerStateMachine.AnimationController.SetTrigger(AnimationController.AnimationState.ChargeRight, resetable: true);
        }

        public void SwitchLeftSword()
        {
            if (m_IsLeftSwordDrawn)
            {
                SheathSword(AnimationController.AnimationState.SheathLeftSword);
            }
            else
            {
                DrawSword(AnimationController.AnimationState.DrawLeftSword);
            }

            m_IsLeftSwordDrawn = !m_IsLeftSwordDrawn;
        }
        
        public void SwitchRightSword()
        {
            if (m_IsRightSwordDrawn)
            {
                SheathSword(AnimationController.AnimationState.SheathRightSword);
            }
            else
            {
                DrawSword(AnimationController.AnimationState.DrawRightSword);
            }

            m_IsRightSwordDrawn = !m_IsRightSwordDrawn;
        }

        public void DrawSword(AnimationController.AnimationState sword)
        {
            m_PlayerStateMachine.AnimationController.SetTrigger(sword);
        }
        
        public void SheathSword(AnimationController.AnimationState sword)
        {
            m_PlayerStateMachine.AnimationController.SetTrigger(sword);
        }

        void OnSpace()
        {
            m_PlayerStateMachine.AnimationController.SetTrigger(AnimationController.AnimationState.Jump);
        }

        void OnRKey()
        {
            m_PlayerStateMachine.AnimationController.SetTrigger(AnimationController.AnimationState.SheathRightSword);
            m_PlayerStateMachine.AnimationController.SetTrigger(AnimationController.AnimationState.Empty);
            m_PlayerStateMachine.ChangeState(m_PlayerStateMachine.FreeRoam);
        }
    
    

        public override void StateMachineUpdate()
        {
            if (m_IsTakingDamage)
            {
                return;
            }
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
            m_PlayerStateMachine.InputManager.RemoveListenerToOnRightLightAttackEvent(OnClick);
            m_PlayerStateMachine.InputManager.RemoveListenerToUnArmEvent(OnRKey);
            m_PlayerStateMachine.InputManager.RemoveListenerToOnLeftLightAttackEvent(OnRightClickDown);
            m_PlayerStateMachine.InputManager.RemoveListenerToOnJumpEvent(OnSpace);
            
            m_PlayerStateMachine.InputManager.RemoveListenerToOnDrawLeftItem(SwitchLeftSword);
            m_PlayerStateMachine.InputManager.RemoveListenerToOnDrawRightItemEvent(SwitchRightSword);
            
            m_PlayerStateMachine.InputManager.RemoveListenerToOnEnableLeftHandBuffEvent(OnChargeLeft);
            m_PlayerStateMachine.InputManager.RemoveListenerToOnEnableRightHandBuffEvent(OnChargeRight);
            
            m_PlayerStateMachine.InputManager.RemoveListenerToOnTwoHandedAttackEvent(OnHeavyAttack);
            
            m_PlayerStateMachine.InputManager.RemoveListenerToOnFKeyDownEvent(OnChargeBoth);
            
            m_PlayerStateMachine.InputManager.RemoveListenerToOnNum3Event(OnHelloEveryone);
            m_PlayerStateMachine.InputManager.RemoveListenerToOnNum4Event(OnStretching);
        }

        private void OnDestroy()
        {
            try
            {
                m_PlayerStateMachine.InputManager.RemoveListenerToOnRightLightAttackEvent(OnClick);
                m_PlayerStateMachine.InputManager.RemoveListenerToUnArmEvent(OnRKey);
                m_PlayerStateMachine.InputManager.RemoveListenerToOnLeftLightAttackEvent(OnRightClickDown);
                m_PlayerStateMachine.InputManager.RemoveListenerToOnJumpEvent(OnSpace);
            
                m_PlayerStateMachine.InputManager.RemoveListenerToOnDrawLeftItem(SwitchLeftSword);
                m_PlayerStateMachine.InputManager.RemoveListenerToOnDrawRightItemEvent(SwitchRightSword);
            
                m_PlayerStateMachine.InputManager.RemoveListenerToOnEnableLeftHandBuffEvent(OnChargeLeft);
                m_PlayerStateMachine.InputManager.RemoveListenerToOnEnableRightHandBuffEvent(OnChargeRight);
            
                m_PlayerStateMachine.InputManager.RemoveListenerToOnTwoHandedAttackEvent(OnHeavyAttack);
            
                m_PlayerStateMachine.InputManager.RemoveListenerToOnFKeyDownEvent(OnChargeBoth);
            
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