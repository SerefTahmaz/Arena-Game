using System;
using UnityEngine;

public class cDragonSoundController : MonoBehaviour
{
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private AudioClip m_ShoutingClip;
    [SerializeField] private AudioClip m_Attack1Clip;
    [SerializeField] private AudioClip m_Attack2Clip;
    [SerializeField] private AudioClip m_FlyBreathingClip;
    [SerializeField] private AudioClip m_MeleeAttackClip;
    [SerializeField] private AudioClip m_MeleeAttack2Clip;
    [SerializeField] private AudioClip m_DeathClip;
    [SerializeField] private AudioClip m_FlyIdleClip;
    [SerializeField] private AudioClip m_TransitionToFlyClip;
    [SerializeField] private AudioClip m_FlyToGroundClip;
    [SerializeField] private AudioClip m_Turn360Clip;
    [SerializeField] private AudioClip m_ForwardJumpClip;
    
    public Action OnPlayShout { get; set; }
    public Action OnPlayAttack2 { get; set; }
    public Action OnPlayAttack1 { get; set; }
    public Action OnPlayFlyBreathingClip { get; set; }
    public Action OnPlayMeleeAttackClip { get; set; }
    public Action OnPlayMeleeAttack2Clip { get; set; }
    public Action OnPlayDeathClip { get; set; }
    public Action OnPlayFlyIdleClip { get; set; }
    public Action OnPlayTransitionToFlyClip { get; set; }
    public Action OnPlayFlyToGroundClip { get; set; }
    public Action OnPlayTurn360Clip { get; set; }
    public Action OnPlayForwardJumpClip { get; set; }
     
    public bool IsInitialized { get; set; }

    public void Init()
    {
        IsInitialized = true;
    }

    public void PlayShout()
    {
        if(!IsInitialized) return;
        PlayShoutAction();
        OnPlayShout?.Invoke();
    }

    public void PlayShoutAction()
    {
        m_AudioSource.pitch = 1;
        m_AudioSource.PlayOneShot(m_ShoutingClip);
    }

    public void PlayAttack2()
    {
        if(!IsInitialized) return;
        PlayAttack2Action();
        OnPlayAttack2?.Invoke();
    }

    public void PlayAttack2Action()
    {
        m_AudioSource.pitch = 1;
        m_AudioSource.PlayOneShot(m_Attack2Clip);
    }

    public void PlayAttack1()
    {
        if(!IsInitialized) return;
        PlayAttack1Action();
        OnPlayAttack1?.Invoke();
    }

    public void PlayAttack1Action()
    {
        m_AudioSource.pitch = 1;
        m_AudioSource.PlayOneShot(m_Attack1Clip);
    }

    public void PlayFlyBreathingClip()
    {
        if(!IsInitialized) return;
        PlayFlyBreathingClipAction();
        OnPlayFlyBreathingClip?.Invoke();
    }

    public void PlayFlyBreathingClipAction()
    {
        m_AudioSource.pitch = 1;
        m_AudioSource.PlayOneShot(m_FlyBreathingClip);
    }

    public void PlayMeleeAttackClip()
    {
        if(!IsInitialized) return;
        PlayMeleeAttackClipAction();
        OnPlayMeleeAttackClip?.Invoke();
    }

    public void PlayMeleeAttackClipAction()
    {
        m_AudioSource.pitch = 1.25f;
        m_AudioSource.PlayOneShot(m_MeleeAttackClip);
    }

    public void PlayMeleeAttack2Clip()
    {
        if(!IsInitialized) return;
        PlayMeleeAttack2ClipAction();
        OnPlayMeleeAttack2Clip?.Invoke();
    }

    public void PlayMeleeAttack2ClipAction()
    {
        m_AudioSource.pitch = 1f;
        m_AudioSource.PlayOneShot(m_MeleeAttack2Clip);
    }

    public void PlayDeathClip()
    {
        if(!IsInitialized) return;
        PlayDeathClipAction();
        OnPlayDeathClip?.Invoke();
    }

    public void PlayDeathClipAction()
    {
        m_AudioSource.pitch = 1;
        m_AudioSource.PlayOneShot(m_DeathClip);
    }

    public void PlayFlyIdleClip()
    {
        if(!IsInitialized) return;
        PlayFlyIdleClipAction();
        OnPlayFlyIdleClip?.Invoke();
    }

    public void PlayFlyIdleClipAction()
    {
        m_AudioSource.pitch = 1;
        m_AudioSource.PlayOneShot(m_FlyIdleClip);
    }

    public void PlayTransitionToFlyClip()
    {
        if(!IsInitialized) return;
        PlayTransitionToFlyClipAction();
        OnPlayTransitionToFlyClip?.Invoke();
    }

    public void PlayTransitionToFlyClipAction()
    {
        m_AudioSource.pitch = 1;
        m_AudioSource.PlayOneShot(m_TransitionToFlyClip);
    }

    public void PlayFlyToGroundClip()
    {
        if(!IsInitialized) return;
        PlayFlyToGroundClipAction();
        OnPlayFlyToGroundClip?.Invoke();
    }

    public void PlayFlyToGroundClipAction()
    {
        m_AudioSource.pitch = 1;
        m_AudioSource.PlayOneShot(m_FlyToGroundClip);
    }

    public void PlayTurn360Clip()
    {
        if(!IsInitialized) return;
        PlayTurn360ClipAction();
        OnPlayTurn360Clip?.Invoke();
    }

    public void PlayTurn360ClipAction()
    {
        m_AudioSource.pitch = 1;
        m_AudioSource.PlayOneShot(m_Turn360Clip);
    }

    public void PlayForwardJumpClip()
    {
        if(!IsInitialized) return;
        PlayForwardJumpClipAction();
        OnPlayForwardJumpClip?.Invoke();
    }

    public void PlayForwardJumpClipAction()
    {
        m_AudioSource.pitch = 1;
        m_AudioSource.PlayOneShot(m_ForwardJumpClip);
    }
}
