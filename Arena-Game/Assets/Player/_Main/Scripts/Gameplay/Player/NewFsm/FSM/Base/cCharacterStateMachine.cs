using FiniteStateMachine;
using UnityEngine;

public abstract class cCharacterStateMachine:cStateMachine
{
    [SerializeField] private cStatesBlackBoard m_BlackBoard;

    // #region Properties

    public cStateBase Empty => m_BlackBoard.m_Empty;

    public cStateBase FreeRoam => m_BlackBoard.m_FreeRoam;

    public cStateBase Fight => m_BlackBoard.m_Fight;

    public cStateBase Dead => m_BlackBoard.m_Dead;
}