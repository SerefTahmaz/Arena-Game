using System;
using FiniteStateMachine;
using PlayerCharacter;
using RootMotion.FinalIK;
using UnityEngine;

public class cTrollCharacter : cCharacter
{
    [SerializeField] private cTrollNetworkController m_TrollNetworkController;
    [SerializeField] private cTrollAnimationController m_AnimationController;
    [SerializeField] private MovementController m_MovementController;
    [SerializeField] private cTrollStateMachine m_TrollStateMachine;

    public override cCharacterNetworkController CharacterNetworkController => TrollNetworkController;
    public cTrollAnimationController AnimationController => m_AnimationController;

    public MovementController MovementController => m_MovementController;

    public cTrollNetworkController TrollNetworkController => m_TrollNetworkController;

    public cTrollStateMachine TrollStateMachine => m_TrollStateMachine;
    
    public override Action OnActionEnded
    {
        get => m_AnimationController.m_OnAttackEnd;
        set => m_AnimationController.m_OnAttackEnd = value;
    }
}