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
        [SerializeField] private float m_FlyDuration;
        [SerializeField] private float m_Distance;
        // [SerializeField] private Transform m_Pivot;

        private Transform m_MovementTransform => StateMachine.Character.MovementTransform;
        
        cDragonStateMachine StateMachine => m_StateMachine as cDragonStateMachine;

        private cAnimationController AnimationController => StateMachine.Character.AnimationController;

        
        public override void Enter()
        {
            base.Enter();
            AnimationController.SetTrigger(cAnimationController.eAttackType.TransitionToFly);

            DOVirtual.DelayedCall(m_FlyStart, () =>
            {
                var pos = m_MovementTransform.position;
                pos += m_MovementTransform.forward * m_Distance;

                Vector3 dir = pos - m_MovementTransform.position;
                dir.y = 0;
                var lookRot = Quaternion.LookRotation(dir.normalized);

                m_MovementTransform.DORotateQuaternion(lookRot, .5f);
                m_MovementTransform.DOMove(pos, m_FlyDuration).SetEase(Ease.InOutQuad).OnComplete((() =>
                {
                    AnimationController.SetTrigger(cAnimationController.eAnimationType.FlyToGround);
                }));
            });
            
            StateMachine.Character.DragonController.m_ActionEnd += OnActionEnd;
        }

        private void OnActionEnd()
        {
            m_StateMachine.ChangeState(StateMachine.m_DragonIdle);
        }
        
        public override void Exit()
        {
            StateMachine.Character.DragonController.m_ActionEnd -= OnActionEnd;
            base.Exit();
        }
        
        private void OnDrawGizmosSelected()
        {
            var color = Gizmos.color;
            Gizmos.color = Color.blue;
            
            var pos = transform.position;
            Gizmos.DrawWireSphere(pos, m_Distance);
            Gizmos.color = color;
        }
    }
}