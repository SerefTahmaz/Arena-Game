using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DemoBlast.Utils;
using UnityEngine;

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

    [SerializeField] private GameObject m_RightDamageEffector;
    [SerializeField] private GameObject m_LeftDamageEffector;

    public void RolImpact()
    {
        m_RollImpact.Play();
    }

    public void RightSlashStart()
    {
        m_RightTrailRenderer.emitting = true;
        // m_RightHandParticles.Play();
        m_RightDamageEffector.SetActive(true);
    }
    
    public void RightSlashEnd()
    {
        m_RightTrailRenderer.emitting = false;
        // m_RightHandParticles.Stop();
        m_RightDamageEffector.SetActive(false);
    }
    
    public void LeftSlashStart()
    {
        m_LeftTrailRenderer.emitting = true;
        // m_LeftHandParticles.Play();
        m_LeftDamageEffector.SetActive(true);
    }
    
    public void LeftSlashEnd()
    {
        m_LeftTrailRenderer.emitting = false;
        // m_LeftHandParticles.Stop();
        m_LeftDamageEffector.SetActive(false);
    }
    
    public void BothSlashStart()
    {
        LeftSlashStart();
        RightSlashStart();
    }
    
    public void BothSlashEnd()
    {
        LeftSlashEnd();
        RightSlashEnd();
    }

    public void OnLeftStep()
    {
        m_LeftStompDustPar.PlayWithClear();
        OnStep();
    }

    public void OnRightStep()
    {
        m_RightStompDustPar.PlayWithClear();
        OnStep();
    }

    public void OnClawSlashStart()
    {
        m_AudioSource.PlayOneShot(m_ClawSlash);
    }
    
    public void OnMutantSwipingStart()
    {
        m_AudioSource.PlayOneShot(m_MutantSwipingClip);
    }
    
    public void OnSpinAttackStart()
    {
        m_AudioSource.PlayOneShot(m_SpinAttackClip);
    }
    
    public void OnComboAttackStart()
    {
        m_AudioSource.PlayOneShot(m_ComboAttackClip);
    }
    
    public void OnRollStart()
    {
        m_AudioSource.PlayOneShot(m_OnRollClip);
    }
    
    public void DeadStart()
    {
        m_AudioSource.PlayOneShot(m_OnDeadClip);
    }
    
    public void HitBodyStart()
    {
        m_AudioSource.PlayOneShot(m_HitBodyClip);
    }

    public void OnStep()
    {
        m_CurrentStep = m_StepAudioClips.Except(new[] { m_CurrentStep }).OrderBy((clip => Random.Range(0, 1000)))
            .FirstOrDefault();
        m_WalkSource.PlayOneShot(m_CurrentStep);
    }
}
