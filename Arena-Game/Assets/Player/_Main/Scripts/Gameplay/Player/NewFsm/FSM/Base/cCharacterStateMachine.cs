using FiniteStateMachine;
using UnityEngine;

public abstract class cCharacterStateMachine:cStateMachine,IDamagable
{
    [SerializeField] private Transform m_FocusTransform;
    public abstract int TeamID { get; }
    public Transform FocusPoint => m_FocusTransform;
}