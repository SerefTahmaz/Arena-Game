using DG.Tweening;
using FiniteStateMachine;

namespace Gameplay.Character.NPCHuman
{
    public class NPCHumanEmpty : Grounded
    {
        NPCHumanStateMachine StateMachine => m_StateMachine as NPCHumanStateMachine;
        
        public override void Enter()
        {
            base.Enter();
            StateMachine.Character.AnimationController.m_OnAttackEnd += ChangeStateToWalk;
        }

        private void ChangeStateToWalk()
        {
            m_StateMachine.ChangeState(StateMachine.Fight);
        }
        
        public override void Exit()
        {
            StateMachine.Character.AnimationController.m_OnAttackEnd -= ChangeStateToWalk;
            base.Exit();
        }
    }
}