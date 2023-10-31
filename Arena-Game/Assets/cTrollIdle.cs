using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace FiniteStateMachine
{
    public class cTrollIdle : Grounded
    {
        cTrollStateMachine StateMachine => m_StateMachine as cTrollStateMachine;
        
        public override void Enter()
        {
            base.Enter();
            StateMachine.TrollCharacter.AnimationController.SetTrigger(cTrollAnimationController.TrollAnimationState.Idle);

            DOVirtual.DelayedCall(.1f, () => StateMachine.ChangeState(StateMachine.m_Walk));
        }

        public override void StateMachineFixedUpdate()
        {
            base.StateMachineFixedUpdate();
        }
        
        public override void Exit()
        {
            base.Exit();
        }
    }
}