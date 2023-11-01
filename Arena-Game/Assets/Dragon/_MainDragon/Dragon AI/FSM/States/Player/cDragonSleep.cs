using System.Collections;
using System.Linq;
using DG.Tweening;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

namespace FiniteStateMachine
{
    public class cDragonSleep : Grounded
    {
        cDragonStateMachine StateMachine => m_StateMachine as cDragonStateMachine;
        
        public override void Enter()
        {
            WakeUp();
            base.Enter();
        }
        
        public override void StateMachineFixedUpdate()
        {
            base.StateMachineFixedUpdate();
            
            // if (Input.GetKeyDown(KeyCode.T))
            // {
            //     WakeUp();
            // }
        }

        public void WakeUp()
        {
            Debug.Log("CALLED");
            StateMachine.Character.AnimationController.SetTrigger(cAnimationController.eAnimationType.Shout);
            StateMachine.Character.DragonAnimationEvents.m_OnDragonShoutEnd += OnShoutEnd;
        }

        public void OnShoutEnd()
        {
            StateMachine.ChangeState(StateMachine.m_DragonIdle);
            StateMachine.DragonCharacter.DragonNetworkController.OnStartFightServerRpc();
            StateMachine.Character.DragonAnimationEvents.m_OnDragonShoutEnd -= OnShoutEnd;
        }
        
        public override void Exit()
        {
            base.Exit();
        }
    }
}