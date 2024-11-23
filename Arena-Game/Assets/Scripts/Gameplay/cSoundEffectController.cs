using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArenaGame.Utils;
using STNest.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

public class cSoundEffectController : MonoBehaviour
{
    [SerializeField] private Rigidbody m_Rigidbody;

    [SerializeField] private LayerMask m_LayerMask;

    [SerializeField] private float m_UpperLimit;
    [SerializeField] private float m_LowerLimit;
    [SerializeField] private float m_StepVolume;

    [SerializeField] private FootHelper m_LeftFoot;
    [SerializeField] private FootHelper m_RightFoot;

    [SerializeField] private List<AudioClip> m_StepClips;

    [SerializeField] private AudioSource m_ChainMailRustle;
    [SerializeField] private float m_VolumeStrenght;

    [SerializeField] private AudioSource m_OneShotSource;
    [SerializeField] private AudioClip m_SwordDraw;

    [SerializeField] private AudioClip m_JumpSound;
    [SerializeField] private AudioClip m_ChargeSwordsSound;
    [SerializeField] private List<AudioClip> m_Grunts;
    [SerializeField] private AudioClip m_DeadSound;

    [Serializable]
    public class FootHelper
    {
        public AudioSource m_AudioSource;
        public Transform m_Foot;
        public bool m_IsAbove;
    }

    [SerializeField] private float m_RustleThreshold;
    
    private bool m_IsPlayingRustle = false;

    private void Update()
    {
       TestFoot(m_LeftFoot);
       TestFoot(m_RightFoot);
       
       var velocityMagnitude = m_Rigidbody.velocity.magnitude;

       if (m_IsPlayingRustle == false && velocityMagnitude > m_RustleThreshold)
       {
           m_IsPlayingRustle = true;
           m_ChainMailRustle.Play();
       }
       else if (m_IsPlayingRustle && velocityMagnitude <= m_RustleThreshold)
       {
           m_IsPlayingRustle = false;
           m_ChainMailRustle.Stop();
       }
       
       m_ChainMailRustle.volume = velocityMagnitude.Remap( 0, 3, 0, 1)*m_VolumeStrenght;
       // m_ChainMailRustle.pitch = ExtensionMethods.Remap(velocityMagnitude, 0, 3, m_RustleLimits.x, m_RustleLimits.y);
    }

    public void TestFoot(FootHelper footHelper)
    {
        if (Physics.Raycast(footHelper.m_Foot.position, Vector3.down, out var hit, 2, m_LayerMask))
        {
            if (hit.distance > m_UpperLimit)
            {
                footHelper.m_IsAbove = true;
            }
            
            if (hit.distance < m_LowerLimit && footHelper.m_IsAbove)
            {
                footHelper.m_IsAbove = false;
                footHelper.m_AudioSource.PlayOneShot(m_StepClips.OrderBy((clip => Random.Range(0,1000))).FirstOrDefault());
                footHelper.m_AudioSource.volume = m_StepVolume;
            }
        }
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

    [SerializeField] private AudioSource m_AudioSourceLoop;

    public void PlayFireChargeLoop(int trackIndex)
    {
        m_AudioSourceLoop.clip = m_FireChargeClips[trackIndex - 1];
        m_AudioSourceLoop.Play();
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
    
    [SerializeField] private AudioSource m_StretchingSource;
    [SerializeField] private AudioClip m_StretchingClip;
    public void PlayStretching()
    {
        PlayOneShot(m_StretchingClip);
    }

    [SerializeField] private AudioSource m_VoiceSource;
    
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
        PlayOneShot(m_DeadSound);
    }
    
    public void PlayOneShot(AudioClip audioClip, float pitch = 1)
    {
        m_OneShotSource.pitch = pitch;
        m_OneShotSource.PlayOneShot(audioClip);
    }
}
