using System;
using System.Collections.Generic;
using System.Linq;
using _Main.Scripts.Gameplay;
using ArenaGame.Utils;
using DefaultNamespace;
using DG.Tweening;
using FiniteStateMachine;
using PlayerCharacter;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Gameplay.Character.NPCHuman
{
    public class PathingNPCDialog : Grounded
    {
        private PathingNPCStateMachine StateMachine => m_StateMachine as PathingNPCStateMachine;
        
        public override void Enter()
        {
            base.Enter();
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