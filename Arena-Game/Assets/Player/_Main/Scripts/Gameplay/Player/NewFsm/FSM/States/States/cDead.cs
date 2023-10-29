using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace FiniteStateMachine
{
    public class cDead : cStateBase
    {
        cPlayerStateMachineV2 StateMachine => m_StateMachine as cPlayerStateMachineV2;
        
        public override void Enter()
        {
            base.Enter();
            StateMachine.Character.AnimationController.SetTrigger(AnimationController.AnimationState.Dead);
            StateMachine.Character.PlayerCharacterNetworkController.OnDeathServerRpc();
        }

        public override void StateMachineUpdate()
        {
            base.StateMachineUpdate();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}