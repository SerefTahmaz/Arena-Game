using UnityEngine;

public class cPlayerCharacter : cCharacter
{
    [SerializeField] private AnimationController m_AnimationController;
    [SerializeField] private cPlayerCharacterNetworkController m_PlayerCharacterNetworkController;
    [SerializeField] private cInventoryManager m_InventoryManager;
    [SerializeField] private cPlayerStateMachineV2 m_PlayerStateMachineV2;
    [SerializeField] private cSoundEffectController m_SoundEffectController;

    public AnimationController AnimationController => m_AnimationController;
    public override cCharacterNetworkController CharacterNetworkController => PlayerCharacterNetworkController;

    public cInventoryManager InventoryManager => m_InventoryManager;

    public cPlayerStateMachineV2 PlayerStateMachineV2 => m_PlayerStateMachineV2;

    public cPlayerCharacterNetworkController PlayerCharacterNetworkController => m_PlayerCharacterNetworkController;

    public cSoundEffectController SoundEffectController => m_SoundEffectController;
}