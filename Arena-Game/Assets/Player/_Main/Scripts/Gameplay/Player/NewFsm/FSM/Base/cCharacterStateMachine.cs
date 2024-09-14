using System.Linq;
using ArenaGame;
using FiniteStateMachine;
using UnityEngine;

public abstract class cCharacterStateMachine:cStateMachine
{
    private NPCTargetHelper m_NpcTargetHelper;
    
    protected override void Awake()
    {
        base.Awake();
        m_NpcTargetHelper = new NPCTargetHelper(this);
    }

    public Transform Target()
    {
        return m_NpcTargetHelper.Target();
    }
}