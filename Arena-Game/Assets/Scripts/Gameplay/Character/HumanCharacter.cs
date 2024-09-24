using System;
using _Main.Scripts.Gameplay;
using ArenaGame.Utils;
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
        [SerializeField] private HumanCharacterStateMachine m_CharacterStateMachine;
        [SerializeField] private CharacterSO m_CharacterSo;
        [SerializeField] private SkinManager m_SkinManager;

        public AnimationController AnimationController => m_AnimationController;
        public override cCharacterNetworkController CharacterNetworkController => PlayerCharacterNetworkController;

        public cInventoryManager InventoryManager => m_InventoryManager;

        public cPlayerCharacterNetworkController PlayerCharacterNetworkController => m_PlayerCharacterNetworkController;

        public cSoundEffectController SoundEffectController => m_SoundEffectController;

        public MovementController MovementController => m_MovementController;

        public HumanCharacterStateMachine CharacterStateMachine => m_CharacterStateMachine;

        public CharacterSO CharacterSo => m_CharacterSo;

        public override int StartHealth => m_CharacterSo.Health;

        public SkinManager SkinManager => m_SkinManager;

        public void OnDamageAnim()
        {
            m_BloodExpo.PlayWithClear();
            SoundEffectController.PlayDamageGrunt();
            // m_DustExpo.PlayWithClear();
        }
    }
}