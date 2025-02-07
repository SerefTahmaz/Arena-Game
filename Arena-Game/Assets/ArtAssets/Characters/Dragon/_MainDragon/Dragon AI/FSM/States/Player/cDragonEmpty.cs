using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace FiniteStateMachine
{
    public class cDragonEmpty : Grounded
    {
        cDragonStateMachine StateMachine => m_StateMachine as cDragonStateMachine;
        
        public override void Enter()
        {
            base.Enter();
            // StateMachine.Character.AnimationController.m_Animator.applyRootMotion = true;
            StateMachine.Character.MovementController.m_EnableFlyingMode = true;
            StateMachine.Character.DragonController.m_ActionEnd += ChangeStateToWalk;

            m_Delay = false;
            DOVirtual.DelayedCall(1, () => m_Delay = true);
        }

        private void ChangeStateToWalk()
        {
            m_StateMachine.ChangeState(StateMachine.m_DragonWalk);
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
            StateMachine.Character.MovementController.m_EnableFlyingMode = false;
            StateMachine.Character.DragonController.m_ActionEnd -= ChangeStateToWalk;
            base.Exit();
        }
    }
}