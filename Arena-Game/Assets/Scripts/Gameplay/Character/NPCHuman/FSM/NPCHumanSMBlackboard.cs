using FiniteStateMachine;
using UnityEngine;

namespace Gameplay.Character.NPCHuman
{
    public class NPCHumanSMBlackboard : MonoBehaviour
    {
        public cStateBase m_Empty;
        public cStateBase m_Fight;
        public cStateBase m_Dead;
    }
}