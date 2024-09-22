using FiniteStateMachine;

namespace Gameplay.Character.NPCHuman
{
    public class NPCHumanDead : cHumanDead
    {
        private NPCHumanStateMachine HumanCharacterStateMachine => m_StateMachine as NPCHumanStateMachine;

        protected override HumanCharacter HumanCharacter => HumanCharacterStateMachine.Character;
    }
}