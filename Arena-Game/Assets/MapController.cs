using System;
using System.Collections;
using System.Collections.Generic;
using AudioSystem;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private SoundData m_StartClipData;
    [SerializeField] private SoundData m_AmbientMusicData;

    private SoundEmitter m_Emitter;
    
    private void Awake()
    {
        MultiplayerLocalHelper.Instance.OnMultiplayerGameStarted += HandleOnGameStarted;
        cGameManager.Instance.OnGameplayEnd += HandleGameEnded;
    }

    private void OnDestroy()
    {
        if (MultiplayerLocalHelper.Instance)
        {
            MultiplayerLocalHelper.Instance.OnMultiplayerGameStarted -= HandleOnGameStarted;
        }

        if (cGameManager.Instance)
        {
            cGameManager.Instance.OnGameplayEnd -= HandleGameEnded;
        }
    }
    
    private void HandleGameEnded()
    {
        if (m_Emitter != null)
        {
            m_Emitter.Stop();
            m_Emitter = null;
        }
    }

    private void HandleOnGameStarted()
    {
        PlayStartMusic();
        PlayAmbientMusic();
    }

    private void PlayAmbientMusic()
    {
        if(m_AmbientMusicData.clip == null) return;
        var soundBuilder = SoundManager.Instance.CreateSoundBuilder();
        m_Emitter=soundBuilder.Play(m_AmbientMusicData);
        m_Emitter.KillAutoRelease();
    }

    private void PlayStartMusic()
    {
        if(m_StartClipData.clip == null) return;
        var soundBuilder = SoundManager.Instance.CreateSoundBuilder();
        soundBuilder.Play(m_StartClipData);
    }
}
