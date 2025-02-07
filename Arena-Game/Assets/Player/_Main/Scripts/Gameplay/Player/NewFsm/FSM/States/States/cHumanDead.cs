using System;
using System.Collections;
using System.Linq;
using _Main.Scripts.Gameplay;
using DefaultNamespace;
using DG.Tweening;
using Gameplay.Character;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace FiniteStateMachine
{
    public abstract class cHumanDead : cDeath
    {
        protected abstract HumanCharacter HumanCharacter { get; }
        
        public override void Enter()
        {
            base.Enter();
            HumanCharacter.CharacterStateMachine.Die();
            HumanCharacter.PlayerCharacterNetworkController.OnDeathServerRpc();
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