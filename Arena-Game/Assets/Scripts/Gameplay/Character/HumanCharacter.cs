using System;
using _Main.Scripts.Gameplay;
using DemoBlast.Utils;
using PlayerCharacter;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Character
{
    public class HumanCharacter : cCharacter
    {
        [SerializeField] private AnimationController m_AnimationController;
        [SerializeField] private cPlayerCharacterNetworkController m_PlayerCharacterNetworkController;
        [SerializeField] private cInventoryManager m_InventoryManager;
        [SerializeField] private cSoundEffectController m_SoundEffectController;
        [SerializeField] private ParticleSystem m_BloodExpo;
        [SerializeField] private MovementController m_MovementController;
        

        public AnimationController AnimationController => m_AnimationController;
        public override cCharacterNetworkController CharacterNetworkController => PlayerCharacterNetworkController;

        public cInventoryManager InventoryManager => m_InventoryManager;

        public cPlayerCharacterNetworkController PlayerCharacterNetworkController => m_PlayerCharacterNetworkController;

        public cSoundEffectController SoundEffectController => m_SoundEffectController;

        public MovementController MovementController => m_MovementController;

        public void OnDamageAnim()
        {
            m_BloodExpo.PlayWithClear();
            SoundEffectController.PlayDamageGrunt();
            // m_DustExpo.PlayWithClear();
        }
    }
}