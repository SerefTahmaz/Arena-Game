using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace FiniteStateMachine
{
    public class cTrollEmpty : Grounded
    {
        cTrollStateMachine StateMachine => m_StateMachine as cTrollStateMachine;
        
        public override void Enter()
        {
            base.Enter();
            // StateMachine.Character.AnimationController.m_Animator.applyRootMotion = true;
            StateMachine.TrollCharacter.AnimationController.m_OnAttackEnd += ChangeStateToWalk;

            m_Delay = false;
            DOVirtual.DelayedCall(1, () => m_Delay = true);
        }

        private void ChangeStateToWalk()
        {
            m_StateMachine.ChangeState(StateMachine.m_Walk);
        }


        private bool m_Delay;
        
        public override void StateMachineFixedUpdate()
        {
            base.StateMachineFixedUpdate();

            // if (StateMachine.Character.AnimationController.CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walk")&&m_Delay)
            // {
            //     m_StateMachine.ChangeState(StateMachine.m_DragonWalk);
            // }
        }
        
        public override void Exit()
        {
            StateMachine.TrollCharacter.AnimationController.m_OnAttackEnd -= ChangeStateToWalk;
            base.Exit();
        }
    }
}