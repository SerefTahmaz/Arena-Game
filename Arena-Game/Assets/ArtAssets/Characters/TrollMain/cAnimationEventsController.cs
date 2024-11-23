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
    
    [SerializeField] private GameObject m_WeaponDamageEffector;
    [SerializeField] private GameObject m_AreaDamageEffector;
    [SerializeField] private cTrollCharacter m_TrollCharacter;

    private AudioClip m_CurrentStep;

    private void Awake()
    {
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
    
    public void OnStep()
    {
        m_CurrentStep = m_StepAudioClips.Except(new[] { m_CurrentStep }).OrderBy((clip => Random.Range(0, 1000)))
            .FirstOrDefault();
        m_WalkSource.PlayOneShot(m_CurrentStep);
    }

    public void OnGroundAttack()
    {
        m_GroundCrackParticle.PlayWithClear();
        CameraManager.Instance.ShakeCamera(10,5,.5f);
    }

    public void InitSwordTrail(string direction)
    {
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
        var pos = m_Hand.position;
        pos.y = transform.position.y;
        m_HammerCrack.transform.position = pos;
        
        m_HammerCrack.Play();
    }

    public void OnAttack1Start()
    { 
        PlayOneShot(m_Attack1);
    }
    
    public void On360AttackStart()
    {
        PlayOneShot(m_360AttackClip);
    }
    
    public void OnHeavyAttack1Start()
    {
        PlayOneShot(m_HeavyAttack1Clip);
    }
    
    public void OnJumpAttackStart()
    {
        PlayOneShot(m_JumpAttackStartClip);
    }
    
    public void OnRoarFrontFace()
    {
        PlayOneShot(m_OnRoarFrontFaceClip);
    }
    
    public void OnRoar()
    {
        m_RoarParticle.PlayWithClear();
    }

    public void StartDamage()
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
        m_WeaponDamageEffector.SetActive(true);
    }
    
    public void DisableWeaponDamageEffector()
    {
        m_WeaponDamageEffector.SetActive(false);
    }
    
    public void EnableAreaDamageEffector()
    {
        m_AreaDamageEffector.SetActive(true);
    }
    
    public void DisableAreaDamageEffector()
    {
        m_AreaDamageEffector.SetActive(false);
    }
}
