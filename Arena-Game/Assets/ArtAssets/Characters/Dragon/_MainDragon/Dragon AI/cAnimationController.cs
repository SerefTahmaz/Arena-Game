using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cAnimationController : MonoBehaviour
{
    [SerializeField] private cCharacter m_Character;
    private Dictionary<eAnimationType, Action> m_AnimationEndEvents = new Dictionary<eAnimationType, Action>();
    private Dictionary<eAnimationType, Action> m_AnimationStartEvents = new Dictionary<eAnimationType, Action>();

    public Dictionary<eAnimationType, Action> AnimationEndEvents => m_AnimationEndEvents;

    public Dictionary<eAnimationType, Action> AnimationStartEvents => m_AnimationStartEvents;

    public Animator CharacterAnimator => m_Character.Animator;

    public enum eAnimationType
    {
        Sleep,
        SleepToIdle,
        Shout,
        Idle,
        Walk,
        LeftTurn,
        RightTurn,
        SnapToIdle,
        FlyIdle,
        FlyToGround,
        Death,
        Forward,
        Count
    }
    
    [Flags]
    public enum eAttackType
    {
        Turn360=1,
        MeleeAttack=2,
        FireBreathing=4,
        FlyBreathing=8,
        ForwardBash=16,
        ForwardJump=32,
        ForwardBashBreathingAttack=64,
        TransitionToFly=128,
        Count=256
    }

    private int[] m_HashId = new int[(int)eAnimationType.Count];

    private Dictionary<eAttackType, int> m_AttackHashes = new Dictionary<eAttackType, int>();

    private void Awake()
    {
        // m_AnimationEndEvents.Add(eAnimationType.Slash, () => {});
        // m_AnimationStartEvents.Add(eAnimationType.Slash, () => {});

        for (var animType = (eAnimationType)0; animType < eAnimationType.Count; ++animType)
        {
            m_HashId[(int)animType] = Animator.StringToHash(animType.ToString());
        }

        foreach (eAttackType VARIABLE in Enum.GetValues(typeof(eAttackType)))
        {
            m_AttackHashes.Add(VARIABLE, Animator.StringToHash(VARIABLE.ToString()));
        }
    }

    public void SetFloat(eAnimationType type, float value)
    {
        CharacterAnimator.SetFloat(m_HashId[(int)type], value);
    }
    
    public void SetInt(eAnimationType type, int value)
    {
        CharacterAnimator.SetInteger(m_HashId[(int)type], value);
    }
    
    public void SetBool(eAnimationType type, bool value)
    {
        CharacterAnimator.SetBool(m_HashId[(int)type], value);
    }
    
    public int GetInt(eAnimationType type)
    {
       return CharacterAnimator.GetInteger(m_HashId[(int)type]);
    }
    
    public float GetFloat(eAnimationType type)
    {
        return CharacterAnimator.GetFloat(m_HashId[(int)type]);
    }

    public void SetTrigger(eAnimationType type)
    {
        CharacterAnimator.SetTrigger(m_HashId[(int)type]);
    }
    
    public void SetTrigger(eAttackType type)
    {
        CharacterAnimator.SetTrigger(m_AttackHashes[type]);
    }

    public void AnimationEnded(eAnimationType mType)
    {
        m_AnimationEndEvents[mType].Invoke();
    }
    
    public void AnimationStarted(eAnimationType mType)
    {
        m_AnimationStartEvents[mType].Invoke();
    }

    public void ResetTrigger(eAnimationType type)
    {
        CharacterAnimator.ResetTrigger(m_HashId[(int)type]);
    }
}