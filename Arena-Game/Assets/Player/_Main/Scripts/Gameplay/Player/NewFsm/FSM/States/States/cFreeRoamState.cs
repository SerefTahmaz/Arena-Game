using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace FiniteStateMachine
{
    public class cFreeRoamState : cStateBase
    {

        cPlayerStateMachineV2 m_PlayerStateMachine => m_StateMachine as cPlayerStateMachineV2;
        private MovementUserController MovementUserController => m_PlayerStateMachine.MovementUserController;

        public override void Enter()
        {
            base.Enter();
            m_PlayerStateMachine.InputManager.AddListenerToOnClickEvent(OnClick);
            m_PlayerStateMachine.InputManager.AddListenerToOnSpaceKeyEvent(OnSpace);
        }
    

        void OnClick()
        {
            m_PlayerStateMachine.ChangeState(m_PlayerStateMachine.Fight);
        }
    
        void OnSpace()
        {
            m_PlayerStateMachine.AnimationController.SetTrigger(AnimationController.AnimationState.Jump);
        }

        public override void StateMachineUpdate()
        {
            base.StateMachineUpdate();
            MovementUserController.Movement();
        }
    
        public override void Exit()
        {
            base.Exit();
            m_PlayerStateMachine.InputManager.RemoveListenerToOnClickEvent(OnClick);
            m_PlayerStateMachine.InputManager.RemoveListenerToOnSpaceKeyEvent(OnSpace);
        }
    }
}