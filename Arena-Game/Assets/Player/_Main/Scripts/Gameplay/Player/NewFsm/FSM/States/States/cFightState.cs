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
            m_PlayerStateMachine.InputManager.AddListenerToOnClickEvent(OnClick);
            m_PlayerStateMachine.InputManager.AddListenerToOnRKeyEvent(OnRKey);
            m_PlayerStateMachine.InputManager.AddListenerToOnRightDownClickEvent(OnRightClickDown);
            m_PlayerStateMachine.InputManager.AddListenerToOnRightUpClickEvent(OnRightClickUp);
            m_PlayerStateMachine.InputManager.AddListenerToOnSpaceKeyEvent(OnSpace);
            
            m_PlayerStateMachine.InputManager.AddListenerToOnNum2Event(SwitchLeftSword);
            m_PlayerStateMachine.InputManager.AddListenerToOnNum1Event(SwitchRightSword);
            
            m_PlayerStateMachine.InputManager.AddListenerToOnCtrlRightClickEvent(OnChargeRight);
            m_PlayerStateMachine.InputManager.AddListenerToOnCtrlLeftClickEvent(OnChargeLeft);
            
            m_PlayerStateMachine.InputManager.AddListenerToOnShiftLeftClickEvent(OnHeavyAttack);
            
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
            m_PlayerStateMachine.AnimationController.SetTrigger(AnimationController.AnimationState.ChargeBoth);
            DOVirtual.DelayedCall(0.01f,
                () => m_PlayerStateMachine.AnimationController.ResetTrigger(AnimationController.AnimationState.ChargeBoth));
        }

        private void OnHeavyAttack()
        {
            m_PlayerStateMachine.AnimationController.SetTrigger(AnimationController.AnimationState.HeavySlash);
            DOVirtual.DelayedCall(0.01f,
                () => m_PlayerStateMachine.AnimationController.ResetTrigger(AnimationController.AnimationState.HeavySlash));
        }

        public void OnChargeLeft()
        {
            m_PlayerStateMachine.AnimationController.SetTrigger(AnimationController.AnimationState.ChargeLeft);
            DOVirtual.DelayedCall(0.01f,
                () => m_PlayerStateMachine.AnimationController.ResetTrigger(AnimationController.AnimationState.ChargeLeft));
        }
        
        public void OnChargeRight()
        {
            m_PlayerStateMachine.AnimationController.SetTrigger(AnimationController.AnimationState.ChargeRight);
            DOVirtual.DelayedCall(0.01f,
                () => m_PlayerStateMachine.AnimationController.ResetTrigger(AnimationController.AnimationState.ChargeRight));
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

        void OnClick()
        {
            m_PlayerStateMachine.AnimationController.SetTrigger(AnimationController.AnimationState.Slash);
            DOVirtual.DelayedCall(0.01f,
                () => m_PlayerStateMachine.AnimationController.ResetTrigger(AnimationController.AnimationState.Slash));
        }
        
        void OnSpace()
        {
            m_PlayerStateMachine.AnimationController.SetTrigger(AnimationController.AnimationState.Jump);
        }
    
        void OnRightClickDown()
        {
            m_PlayerStateMachine.AnimationController.SetTrigger(AnimationController.AnimationState.LeftSlash);
            DOVirtual.DelayedCall(0.01f,
                () => m_PlayerStateMachine.AnimationController.ResetTrigger(AnimationController.AnimationState.LeftSlash));
        }
    
        void OnRightClickUp()
        {
            m_PlayerStateMachine.AnimationController.SetTrigger(AnimationController.AnimationState.ClashEmpty);
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
            m_PlayerStateMachine.InputManager.RemoveListenerToOnClickEvent(OnClick);
            m_PlayerStateMachine.InputManager.RemoveListenerToOnRKeyEvent(OnRKey);
            m_PlayerStateMachine.InputManager.RemoveListenerToOnRightDownClickEvent(OnRightClickDown);
            m_PlayerStateMachine.InputManager.RemoveListenerToOnRightUpClickEvent(OnRightClickUp);
            m_PlayerStateMachine.InputManager.RemoveListenerToOnSpaceKeyEvent(OnSpace);
            
            m_PlayerStateMachine.InputManager.RemoveListenerToOnNum2Event(SwitchLeftSword);
            m_PlayerStateMachine.InputManager.RemoveListenerToOnNum1Event(SwitchRightSword);
            
            m_PlayerStateMachine.InputManager.RemoveListenerToOnCtrlRightClickEvent(OnChargeRight);
            m_PlayerStateMachine.InputManager.RemoveListenerToOnCtrlLeftClickEvent(OnChargeLeft);
            
            m_PlayerStateMachine.InputManager.RemoveListenerToOnShiftLeftClickEvent(OnHeavyAttack);
            
            m_PlayerStateMachine.InputManager.RemoveListenerToOnFKeyDownEvent(OnChargeBoth);
            
            m_PlayerStateMachine.InputManager.RemoveListenerToOnNum3Event(OnHelloEveryone);
            m_PlayerStateMachine.InputManager.RemoveListenerToOnNum4Event(OnStretching);
        }
    }
}