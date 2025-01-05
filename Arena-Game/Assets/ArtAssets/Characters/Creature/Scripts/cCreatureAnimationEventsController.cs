using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArenaGame.Utils;
using STNest.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

public class cCreatureAnimationEventsController : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_LeftStompDustPar;
    [SerializeField] private ParticleSystem m_RightStompDustPar;

    [SerializeField] private AudioClip m_ClawSlash;
    [SerializeField] private AudioClip m_MutantSwipingClip;
    [SerializeField] private AudioClip m_SpinAttackClip;
    [SerializeField] private AudioClip m_ComboAttackClip;
    [SerializeField] private AudioClip m_OnRollClip;
    [SerializeField] private AudioClip m_OnDeadClip;
    [SerializeField] private AudioClip m_HitBodyClip;
    [SerializeField] private AudioSource m_AudioSource;

    [SerializeField] private AudioSource m_WalkSource;
    
    [SerializeField] private List<AudioClip> m_StepAudioClips;
    private AudioClip m_CurrentStep;

    [SerializeField] private TrailRenderer m_RightTrailRenderer;
    
    [SerializeField] private TrailRenderer m_LeftTrailRenderer;

    [SerializeField] private ParticleSystem m_RollImpact;

    [SerializeField] private cDamageEffectorBase m_RightDamageEffector;
    [SerializeField] private cDamageEffectorBase m_LeftDamageEffector;
    
    public Action OnRolImpact { get; set; }
    public Action OnRightSlashStart { get; set; }
    public Action OnRightSlashEnd { get; set; }
    public Action OnLeftSlashStart { get; set; }
    public Action OnBothSlashStart { get; set; }
    public Action OnBothSlashEnd { get; set; }
    public Action OnLeftStepEvent { get; set; }
    public Action OnRightStepEvent { get; set; }
    public Action OnClawSlashStartEvent { get; set; }
    public Action OnMutantSwipingStartEvent { get; set; }
    public Action OnSpinAttackStartEvent { get; set; }
    public Action OnComboAttackStartEvent { get; set; }
    public Action OnRollStartEvent { get; set; }
    public Action OnDeadStart { get; set; }
    public Action OnHitBodyStart { get; set; }
        
    public bool IsInitialized { get; set; }

    public void Init()
    {
        IsInitialized = true;
    }

    public void RolImpact()
    {
        if(!IsInitialized) return;
        RolImpactAction();
        OnRolImpact?.Invoke();
    }

    public void RolImpactAction()
    {
        m_RollImpact.Play();
    }

    public void RightSlashStart()
    {
        if(!IsInitialized) return;
        RightSlashStartAction();
        OnRightSlashStart?.Invoke();
    }

    public void RightSlashStartAction()
    {
        m_RightTrailRenderer.emitting = true;
        // m_RightHandParticles.Play();
        m_RightDamageEffector.SetActiveDamage(true);
    }

    public void RightSlashEnd()
    {
        if(!IsInitialized) return;
        RightSlashEndAction();
        OnRightSlashEnd?.Invoke();
    }

    public void RightSlashEndAction()
    {
        m_RightTrailRenderer.emitting = false;
        // m_RightHandParticles.Stop();
        m_RightDamageEffector.SetActiveDamage(false);
    }

    public void LeftSlashStart()
    {
        if(!IsInitialized) return;
        LeftSlashStartAction();
    }

    public void LeftSlashStartAction()
    {
        m_LeftTrailRenderer.emitting = true;
        // m_LeftHandParticles.Play();
        m_LeftDamageEffector.SetActiveDamage(true);
    }

    public void LeftSlashEnd()
    {
        if(!IsInitialized) return;
        LeftSlashEndAction();
        OnLeftSlashStart?.Invoke();
    }

    public void LeftSlashEndAction()
    {
        m_LeftTrailRenderer.emitting = false;
        // m_LeftHandParticles.Stop();
        m_LeftDamageEffector.SetActiveDamage(false);
    }

    public void BothSlashStart()
    {
        if(!IsInitialized) return;
        BothSlashStartAction();
        OnBothSlashStart?.Invoke();
    }

    public void BothSlashStartAction()
    {
        LeftSlashStart();
        RightSlashStart();
    }

    public void BothSlashEnd()
    {
        if(!IsInitialized) return;
        BothSlashEndAction();
        OnBothSlashEnd?.Invoke();
    }

    public void BothSlashEndAction()
    {
        LeftSlashEnd();
        RightSlashEnd();
    }

    public void OnLeftStep()
    {
        if(!IsInitialized) return;
        OnLeftStepAction();
        OnLeftStepEvent?.Invoke();
    }

    public void OnLeftStepAction()
    {
        m_LeftStompDustPar.PlayWithClear();
        OnStep();
    }

    public void OnRightStep()
    {
        if(!IsInitialized) return;
        OnRightStepAction();
        OnRightStepEvent?.Invoke();
    }

    public void OnRightStepAction()
    {
        m_RightStompDustPar.PlayWithClear();
        OnStep();
    }

    public void OnClawSlashStart()
    {
        if(!IsInitialized) return;
        OnClawSlashStartAction();
        OnClawSlashStartEvent?.Invoke();
    }

    public void OnClawSlashStartAction()
    {
        PlayOneShot(m_ClawSlash);
    }

    public void OnMutantSwipingStart()
    {
        if(!IsInitialized) return;
        OnMutantSwipingStartAction();
        OnMutantSwipingStartEvent?.Invoke();
    }

    public void OnMutantSwipingStartAction()
    {
        PlayOneShot(m_MutantSwipingClip);
    }

    public void OnSpinAttackStart()
    {
        if(!IsInitialized) return;
        OnSpinAttackStartAction();
        OnSpinAttackStartEvent?.Invoke();
    }

    public void OnSpinAttackStartAction()
    {
        PlayOneShot(m_SpinAttackClip);
    }

    public void OnComboAttackStart()
    {
        if(!IsInitialized) return;
        OnComboAttackStartAction();
        OnComboAttackStartEvent?.Invoke();
    }

    public void OnComboAttackStartAction()
    {
        PlayOneShot(m_ComboAttackClip);
    }

    public void OnRollStart()
    {
        if(!IsInitialized) return;
        OnRollStartAction();
        OnRollStartEvent?.Invoke();
    }

    public void OnRollStartAction()
    {
        PlayOneShot(m_OnRollClip);
    }

    public void DeadStart()
    {
        if(!IsInitialized) return;
        DeadStartAction();
        OnDeadStart?.Invoke();
    }

    public void DeadStartAction()
    {
        PlayOneShot(m_OnDeadClip);
    }

    public void HitBodyStart()
    {
        if(!IsInitialized) return;
        HitBodyStartAction();
        OnHitBodyStart?.Invoke();
    }

    public void HitBodyStartAction()
    {
        PlayOneShot(m_HitBodyClip, Helpers.RandomPentatonicPitch());
    }

    public void PlayOneShot(AudioClip audioClip, float pitch = 1)
    {
        m_AudioSource.pitch = pitch;
        m_AudioSource.PlayOneShot(audioClip);
    }

    public void OnStep()
    {
        m_CurrentStep = m_StepAudioClips.Except(new[] { m_CurrentStep }).OrderBy((clip => Random.Range(0, 1000)))
            .FirstOrDefault();
        m_WalkSource.PlayOneShot(m_CurrentStep);
    }
}
