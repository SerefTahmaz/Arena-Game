using Gameplay.Character;

namespace FiniteStateMachine
{
    public class PlayerDead : cHumanDead
    {
        private cPlayerStateMachineV2 PlayerStateMachine => m_StateMachine as cPlayerStateMachineV2;

        protected override HumanCharacter HumanCharacter => PlayerStateMachine.Character;
    }
} 