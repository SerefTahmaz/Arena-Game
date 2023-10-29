using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class cDragonSoundController : MonoBehaviour
{
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private AudioSource m_ShoutingAudioSource;
    [SerializeField] private AudioSource m_BossAudioSource;
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
    [SerializeField] private AudioClip m_BossMusic;


    [ClientRpc]
    public void PlayBossMusicClientRpc()
    {
        m_BossAudioSource.PlayOneShot(m_BossMusic);
    }
    
    public void StopBossMusic()
    {
        m_BossAudioSource.Stop();
    }

    public void PlayShout()
    {
        m_AudioSource.pitch = 1;
        m_ShoutingAudioSource.PlayOneShot(m_ShoutingClip);
    }
    
    public void PlayAttack2()
    {
        m_AudioSource.pitch = 1;
        m_AudioSource.PlayOneShot(m_Attack2Clip);
    }
    
    public void PlayAttack1()
    {
        m_AudioSource.pitch = 1;
        m_AudioSource.PlayOneShot(m_Attack1Clip);
    }
    
    public void PlayFlyBreathingClip()
    {
        m_AudioSource.pitch = 1;
        m_AudioSource.PlayOneShot(m_FlyBreathingClip);
    }
    
    public void PlayMeleeAttackClip()
    {
        m_AudioSource.pitch = 1.25f;
        m_AudioSource.PlayOneShot(m_MeleeAttackClip);
    }
    
    public void PlayMeleeAttack2Clip()
    {
        m_AudioSource.pitch = 1f;
        m_AudioSource.PlayOneShot(m_MeleeAttack2Clip);
    }
    
    public void PlayDeathClip()
    {
        m_AudioSource.pitch = 1;
        m_AudioSource.PlayOneShot(m_DeathClip);
    }
    
    public void PlayFlyIdleClip()
    {
        m_AudioSource.pitch = 1;
        m_AudioSource.PlayOneShot(m_FlyIdleClip);
    }
    
    public void PlayTransitionToFlyClip()
    {
        m_AudioSource.pitch = 1;
        m_AudioSource.PlayOneShot(m_TransitionToFlyClip);
    }
    
    public void PlayFlyToGroundClip()
    {
        m_AudioSource.pitch = 1;
        m_AudioSource.PlayOneShot(m_FlyToGroundClip);
    }
    
    public void PlayTurn360Clip()
    {
        m_AudioSource.pitch = 1;
        m_AudioSource.PlayOneShot(m_Turn360Clip);
    }
    
    public void PlayForwardJumpClip()
    {
        m_AudioSource.pitch = 1;
        m_AudioSource.PlayOneShot(m_ForwardJumpClip);
    }

    public void StopAmbient()
    {
        // m_AmbientSource.Stop();
    }
}
