using RootMotion.FinalIK;
using UnityEngine;

public class cDragonCharacter : cCharacter
{
    [SerializeField] private cDragonController m_DragonController;
    [SerializeField] private LookAtIK m_HeadLookAtIk;
    [SerializeField] private cDragonSoundController m_DragonSoundController;
    [SerializeField] private cAnimationController m_AnimationController;
    [SerializeField] private cDragonNetworkController m_DragonNetworkController;
    [SerializeField] private cDragonAnimationEvents m_DragonAnimationEvents;

    public cDragonController DragonController => m_DragonController;
    public LookAtIK HeadLookAtIk => m_HeadLookAtIk;
    public cDragonSoundController DragonSoundController => m_DragonSoundController;
    public cAnimationController AnimationController => m_AnimationController;

    public override cCharacterNetworkController CharacterNetworkController => DragonNetworkController;
    public cDragonNetworkController DragonNetworkController => m_DragonNetworkController;

    public cDragonAnimationEvents DragonAnimationEvents => m_DragonAnimationEvents;
}