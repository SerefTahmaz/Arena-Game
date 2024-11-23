using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace FiniteStateMachine
{
    public class cTrollDeath : cDeath
    {
        cTrollStateMachine StateMachine => m_StateMachine as cTrollStateMachine;

        private cTrollAnimationController AnimationController => StateMachine.TrollCharacter.AnimationController;
        
        public override void Enter()
        {
            base.Enter();
            AnimationController.SetTrigger(cTrollAnimationController.TrollAnimationState.Dead);
           
            DOVirtual.Float(0, 1, 1, value =>
            {
                StateMachine.TrollCharacter.MovementController.Move(Vector3.zero);
            });
            Debug.Log("Stopping Movement");
            cGameManager.Instance.m_OnNpcDied.Invoke();
            // StateMachine.Character.DragonNetworkController.OnEndFightServerRpc();
            StateMachine.TrollCharacter.TrollNetworkController.OnDeathServerRpc();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}