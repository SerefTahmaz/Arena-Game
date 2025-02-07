using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace FiniteStateMachine
{
    public class cDragonFly : Grounded
    {
        [SerializeField] private float m_FlyStart;
        // [SerializeField] private Transform m_Pivot;

        private Transform m_MovementTransform => StateMachine.Character.MovementTransform;
        
        cDragonStateMachine StateMachine => m_StateMachine as cDragonStateMachine;

        private cAnimationController AnimationController => StateMachine.Character.AnimationController;

        
        public override void Enter()
        {
            base.Enter();
            AnimationController.SetTrigger(cAnimationController.eAttackType.TransitionToFly);
            StateMachine.Character.MovementController.m_EnableFlyingMode = true;

            DOVirtual.DelayedCall(m_FlyStart, () =>
            {
                var pos = m_MovementTransform.position;
                pos += m_MovementTransform.forward * 100;

                Vector3 dir = pos - m_MovementTransform.position;
                dir.y = 0;
                var lookRot = Quaternion.LookRotation(dir.normalized);

                m_MovementTransform.DORotateQuaternion(lookRot, .5f);
                DOVirtual.DelayedCall(Random.Range(3.0f, 7f), () =>
                {
                    AnimationController.SetTrigger(cAnimationController.eAnimationType.FlyToGround);
                });
            });
            
            StateMachine.Character.DragonController.m_ActionEnd += OnActionEnd;
        }

        private void OnActionEnd()
        {
            m_StateMachine.ChangeState(StateMachine.m_DragonIdle);
        }
        
        public override void Exit()
        {
            StateMachine.Character.MovementController.m_EnableFlyingMode = false;
            StateMachine.Character.DragonController.m_ActionEnd -= OnActionEnd;
            base.Exit();
        }
        
        // private void OnDrawGizmosSelected()
        // {
        //     var color = Gizmos.color;
        //     Gizmos.color = Color.blue;
        //     
        //     var pos = transform.position;
        //     Gizmos.color = color;
        // }
    }
}