using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace FiniteStateMachine
{
    public class cDragonIdle : Grounded
    {
        cDragonStateMachine StateMachine => m_StateMachine as cDragonStateMachine;
        
        public override void Enter()
        {
            base.Enter();
            StateMachine.Character.AnimationController.SetTrigger(cAnimationController.eAnimationType.Idle);

            DOVirtual.DelayedCall(.1f, () => StateMachine.ChangeState(StateMachine.m_DragonWalk));
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