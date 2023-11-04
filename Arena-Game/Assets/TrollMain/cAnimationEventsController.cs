using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DemoBlast.Utils;
using UnityEngine;

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
    [SerializeField] private AudioSource m_AudioSource;

    [SerializeField] private AudioSource m_WalkSource;
    
    [SerializeField] private List<AudioClip> m_StepAudioClips;
    
    [SerializeField] private GameObject m_WeaponDamageEffector;
    [SerializeField] private GameObject m_AreaDamageEffector;

    private AudioClip m_CurrentStep;

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
        FindObjectOfType<cCamShake>().ShakeCamera(10,5,.5f);
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
        m_AudioSource.PlayOneShot(m_Attack1);
    }
    
    public void On360AttackStart()
    {
        m_AudioSource.PlayOneShot(m_360AttackClip);
    }
    
    public void OnHeavyAttack1Start()
    {
        m_AudioSource.PlayOneShot(m_HeavyAttack1Clip);
    }
    
    public void OnJumpAttackStart()
    {
        m_AudioSource.PlayOneShot(m_JumpAttackStartClip);
    }
    
    public void OnRoarFrontFace()
    {
        m_AudioSource.PlayOneShot(m_OnRoarFrontFaceClip);
    }
    
    public void OnRoar()
    {
        m_RoarParticle.PlayWithClear();
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
