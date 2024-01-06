using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace FiniteStateMachine
{
    public class cDragonDeath : cDeath
    {
        private Transform m_MovementTransform => StateMachine.Character.MovementTransform;
        
        cDragonStateMachine StateMachine => m_StateMachine as cDragonStateMachine;

        private cAnimationController AnimationController => StateMachine.Character.AnimationController;
        
        public override void Enter()
        {
            base.Enter();
            AnimationController.SetTrigger(cAnimationController.eAnimationType.Death);
            cGameManager.Instance.m_OnNpcDied.Invoke();
            StateMachine.Character.DragonNetworkController.OnDeathServerRpc();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}