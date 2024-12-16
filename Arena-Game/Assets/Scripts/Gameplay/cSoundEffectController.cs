using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArenaGame.Utils;
using AudioSystem;
using RootMotion.FinalIK;
using STNest.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class cSoundEffectController : MonoBehaviour
{
    [SerializeField] private LayerMask m_StepCheckLayerMask;
    [SerializeField] private float m_StepRaycastThreshold;
    [SerializeField] private float m_StepStartYPos;
    [SerializeField] private float m_SphereSize;
    [SerializeField] SoundData m_StepSoundData;
    [SerializeField] private Transform m_LeftFoot;
    [SerializeField] private Transform m_RightFoot; 
    [SerializeField] private List<AudioClip> m_StepClips;

    [SerializeField] SoundData m_BaseSoundData;
    [SerializeField] private AudioClip m_SwordDraw;
    [SerializeField] private AudioClip m_JumpSound;
    [SerializeField] private AudioClip m_ChargeSwordsSound;
    [SerializeField] private List<AudioClip> m_Grunts;
    [SerializeField] private List<AudioClip> m_DeadSounds;

    public void OnRightStep()
    {
        // Debug.Log("Test Right Step");
        TestFoot(m_LeftFoot);
    }
    
    public void OnLeftStep()
    {
        // Debug.Log("Test Left Step");
        TestFoot(m_RightFoot);
    }

    private Collider[] volumeCols = new Collider[5];
    
    private void TestFoot(Transform footRef)
    {
        var size = Physics.OverlapSphereNonAlloc(footRef.position + Vector3.up * m_StepStartYPos, m_SphereSize, volumeCols, m_StepCheckLayerMask);
        if (size > 0)
        {
            m_StepSoundData.clip = FootStepHelper.Instance.GetClips(volumeCols[0]);
            SoundBuilder soundBuilder = SoundManager.Instance.CreateSoundBuilder();
                
            soundBuilder
                .WithRandomPitch()
                .WithPosition(footRef.position)
                .Play(m_StepSoundData);
            return;
        }
        
        Vector3 end = footRef.position + Vector3.up * m_StepStartYPos + Vector3.down * m_StepRaycastThreshold;
        if (Physics.SphereCast(new Ray(footRef.position + Vector3.up*m_StepStartYPos, Vector3.down),m_SphereSize, out var hit, m_StepRaycastThreshold, m_StepCheckLayerMask))
        {
            end = hit.point;
            m_StepSoundData.clip = FootStepHelper.Instance.GetClips(hit.collider);
            SoundBuilder soundBuilder = SoundManager.Instance.CreateSoundBuilder();
                
            soundBuilder
                .WithRandomPitch()
                .WithPosition(footRef.position)
                .Play(m_StepSoundData);
        }
        Debug.DrawLine(footRef.position + Vector3.up*m_StepStartYPos,end ,Color.red,0.1f);
    }

    public void PlaySwordDraw()
    {
        PlayOneShot(m_SwordDraw);
    }

    [SerializeField] private List<AudioClip> m_DlashClips;

    public void PlayDSlash(int trackIndex)
    {
        PlayOneShot(m_DlashClips[trackIndex-1]);
    }
    
    [SerializeField] private List<AudioClip> m_FireChargeClips;

    public void PlayFireCharge(int trackIndex)
    {
        PlayOneShot(m_FireChargeClips[trackIndex-1]);
    }
    
    [SerializeField] private List<AudioClip> m_DualAttackClips;

    public void PlayDualAttack(int trackIndex)
    {
        PlayOneShot(m_DualAttackClips[trackIndex-1]);
    }

    public void PlayJumpSound()
    {
        PlayOneShot(m_JumpSound);
    }
    
    public void PlayChargeSwordsSound()
    {
        PlayOneShot(m_ChargeSwordsSound);
    }
    
    [SerializeField] private AudioClip m_StretchingClip;
    public void PlayStretching()
    {
        PlayOneShot(m_StretchingClip);
    }

    
    [SerializeField] private AudioClip m_HelloEveryone;

    public void PlayHelloEveryone()
    {
        PlayOneShot(m_HelloEveryone);
    }

    public void PlayDamageGrunt()
    {
        PlayOneShot(m_Grunts.RandomItem());
    }
    
    public void PlayDead()
    {
        foreach (var deathClip in m_DeadSounds)
        {
            PlayOneShot(deathClip);
        }
    }
    
    public void PlayOneShot(AudioClip audioClip)
    {
        m_StepSoundData.clip = audioClip;
        SoundBuilder soundBuilder = SoundManager.Instance.CreateSoundBuilder();

        soundBuilder
            .WithPosition(transform.position)
            .Play(m_StepSoundData);
    }
}
