using System.Linq;
using ArenaGame;
using FiniteStateMachine;
using UnityEngine;

public abstract class cCharacterStateMachine:cStateMachine
{
    private NPCTargetHelper m_NpcTargetHelper;

    public NPCTargetHelper NpcTargetHelper => m_NpcTargetHelper;

    protected override void Awake()
    {
        base.Awake();
        m_NpcTargetHelper = new NPCTargetHelper(this);
    }

    public Transform Target()
    {
        return NpcTargetHelper.Target();
    }
}