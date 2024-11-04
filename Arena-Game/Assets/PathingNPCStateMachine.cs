using System.Collections.Generic;
using System.Linq;
using _Main.Scripts.Gameplay;
using ArenaGame.Utils;
using DefaultNamespace;
using FiniteStateMachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Character.NPCHuman
{
    public class PathingNPCStateMachine : cCharacterStateMachine
    {
        #region PritaveFields
        
        [SerializeField] private PathingNPCSMBlackboard m_BlackBoard;
        [SerializeField] private PathingCharacter m_Character;
        [SerializeField] private PatrolPath m_PatrolPath;

        // #region Properties

        public cStateBase Empty => m_BlackBoard.m_Empty;
        public cStateBase Patrol => m_BlackBoard.m_Patrol;
        public cStateBase Dialog => m_BlackBoard.m_Dialog;

        #endregion

        #region States

        #endregion

        #region Properties

        public PathingCharacter Character => m_Character;

        #endregion

        public override int TeamID { get; }

        public PatrolPath PatrolPath => m_PatrolPath;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            Empty.InitializeState("Empty", this);
            Patrol.InitializeState("Patrol", this);
            Dialog.InitializeState("Dialog", this);
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override cStateBase GetInitialState()
        {
            return Patrol;
        }
    }
}
