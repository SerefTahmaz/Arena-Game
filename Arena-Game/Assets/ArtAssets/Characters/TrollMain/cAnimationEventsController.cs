using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArenaGame.Utils;
using STNest.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

public class cAnimationEventsController : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_LeftStompDustPar;
    [SerializeField] private ParticleSystem m_RightStompDustPar;
    [SerializeField] private ParticleSystem m_GroundCrackParticle;

    [SerializeField] private Transform m_Hand;
    [SerializeField] private ParticleSystem m_SwordTrail;
    [SerializeField] private ParticleSystem m_HammerCrack;
    [SerializeField] private ParticleSystem m_RoarParticle;

    [SerializeField] private AudioClip m_Attack1;
    [SerializeField] private AudioClip m_360AttackClip;
    [SerializeField] private AudioClip m_HeavyAttack1Clip;
    [SerializeField] private AudioClip m_JumpAttackStartClip;
    [SerializeField] private AudioClip m_OnRoarFrontFaceClip;
    [SerializeField] private AudioClip m_DamageSound;
    [SerializeField] private AudioSource m_AudioSource;

    [SerializeField] private AudioSource m_WalkSource;
    
    [SerializeField] private List<AudioClip> m_StepAudioClips;
    
    [SerializeField] private cDamageEffector m_WeaponDamageEffector;
    [SerializeField] private cDamageEffector m_AreaDamageEffector;

    private AudioClip m_CurrentStep;
    
    public Action OnLeftStepEvent { get; set; }
    public Action OnRightStepEvent { get; set; }
    public Action OnGroundAttackEvent { get; set; }
    public Action<string> OnInitSwordTrail { get; set; } 
    public Action OnInitHammerCrack { get; set; }
    public Action OnAttack1StartEvent { get; set; }
    public Action On360AttackStartEvent { get; set; }
    public Action OnHeavyAttack1StartEvent { get; set; }
    public Action OnJumpAttackStartEvent { get; set; }
    public Action OnRoarFrontFaceEvent { get; set; }
    public Action OnRoarEvent { get; set; }
    public Action OnStartDamage { get; set; }

    public bool IsInitialized { get; set; }

    public void Init()
    {
        IsInitialized = true;
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

    private void OnStep()
    {
        m_CurrentStep = m_StepAudioClips.Except(new[] { m_CurrentStep }).OrderBy((clip => Random.Range(0, 1000)))
            .FirstOrDefault();
        m_WalkSource.PlayOneShot(m_CurrentStep);
    }

    public void OnGroundAttack()
    {
        if(!IsInitialized) return;
        OnGroundAttackAction();
        OnGroundAttackEvent?.Invoke();
    }

    public void OnGroundAttackAction()
    {
        m_GroundCrackParticle.PlayWithClear();
        CameraManager.Instance.ShakeCamera(10,5,.5f);
    }

    public void InitSwordTrail(string direction)
    {
        if(!IsInitialized) return;
        InitSwordTrailAction(direction);
        Debug.Log($"Direction before {direction}");
        OnInitSwordTrail?.Invoke(direction);
    }

    public void InitSwordTrailAction(string direction)
    {
        Debug.Log($"Direction after {direction}");
        var dir = transform.TransformDirection(new Vector3(-1+int.Parse(direction[0].ToString()),-1+int.Parse(direction[1].ToString())
            ,-1+int.Parse(direction[2].ToString())));
        m_SwordTrail.transform.forward = dir;
        var pos = m_Hand.position;
        pos.y = transform.position.y;
        m_SwordTrail.transform.position = pos;
        
        m_SwordTrail.Play();
    }

    public void InitHammerCrack()
    {
        if(!IsInitialized) return;

        InitHammerCrackAction();
        OnInitHammerCrack?.Invoke();
        
        Debug.Log("Hammer crack!");
    }

    public void InitHammerCrackAction()
    {
        var pos = m_Hand.position;
        pos.y = transform.position.y;
        m_HammerCrack.transform.position = pos;
        
        m_HammerCrack.Play();
    }

    public void OnAttack1Start()
    {
        if(!IsInitialized) return;
        OnAttack1StartAction();
        OnAttack1StartEvent?.Invoke();
    }

    public void OnAttack1StartAction()
    {
        PlayOneShot(m_Attack1);
    }

    public void On360AttackStart()
    {
        if(!IsInitialized) return;
        On360AttackStartAction();
        On360AttackStartEvent?.Invoke();
    }

    public void On360AttackStartAction()
    {
        PlayOneShot(m_360AttackClip);
    }

    public void OnHeavyAttack1Start()
    {
        if(!IsInitialized) return;
        OnHeavyAttack1StartAction();
        OnHeavyAttack1StartEvent?.Invoke();
    }

    public void OnHeavyAttack1StartAction()
    {
        PlayOneShot(m_HeavyAttack1Clip);
    }

    public void OnJumpAttackStart()
    {
        if(!IsInitialized) return;
        OnJumpAttackStartAction();
        OnJumpAttackStartEvent?.Invoke();
    }

    public void OnJumpAttackStartAction()
    {
        PlayOneShot(m_JumpAttackStartClip);
    }

    public void OnRoarFrontFace()
    {
        if(!IsInitialized) return;
        OnRoarFrontFaceAction();
        OnRoarFrontFaceEvent?.Invoke();
    }

    public void OnRoarFrontFaceAction()
    {
        PlayOneShot(m_OnRoarFrontFaceClip);
    }

    public void OnRoar()
    {
        if(!IsInitialized) return;
        OnRoarAction();
        OnRoarEvent?.Invoke();
    }

    public void OnRoarAction()
    {
        m_RoarParticle.PlayWithClear();
    }

    public void StartDamage()
    {
        if(!IsInitialized) return;
        StartDamageAction();
        OnStartDamage?.Invoke();
    }

    public void StartDamageAction()
    {
        m_AudioSource.Stop();
        PlayOneShot(m_DamageSound, Helpers.RandomPentatonicPitch());
    }

    public void PlayOneShot(AudioClip audioClip, float pitch = 1)
    {
        m_AudioSource.pitch = pitch;
        m_AudioSource.PlayOneShot(audioClip);
    }
    
    public void EnableWeaponDamageEffector()
    {
        m_WeaponDamageEffector.SetActiveDamage(true);
    }
    
    public void DisableWeaponDamageEffector()
    {
        m_WeaponDamageEffector.SetActiveDamage(false);
    }
    
    public void EnableAreaDamageEffector()
    {
        m_AreaDamageEffector.SetActiveDamage(true);
    }
    
    public void DisableAreaDamageEffector()
    {
        m_AreaDamageEffector.SetActiveDamage(false);
    }
}