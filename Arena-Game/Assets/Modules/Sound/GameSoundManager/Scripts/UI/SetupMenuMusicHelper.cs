using System.Collections;
using System.Collections.Generic;
using ArenaGame.UI;
using AudioSystem;
using UnityEngine;
using UnityEngine.Audio;

public class SetupMenuMusicHelper : MonoBehaviour
{
    [SerializeField] private cView m_View;
    [SerializeField] private AudioClip m_SetupMenuMusicClip;
    [SerializeField] private float m_Volume;
        
    private SoundEmitter m_SetupMenuMusicEmitter;
        
    private void Awake()
    {
        m_View.OnActivateEvent.AddListener(MainMenuEnter);
        m_View.OnDeActivateEvent.AddListener(MainMenuExit);
    }
        
    private void OnDestroy()
    {
        if (m_View != null)
        {
            m_View.OnActivateEvent.RemoveListener(MainMenuEnter);
            m_View.OnDeActivateEvent.RemoveListener(MainMenuExit);
        }
    }

    private void MainMenuEnter()
    {
        m_SetupMenuMusicEmitter = SoundManager.PlayOneShot2DMusic(m_SetupMenuMusicClip, m_Volume);
        m_SetupMenuMusicEmitter.KillAutoRelease();
    }
    
    private void MainMenuExit()
    {
        if (m_SetupMenuMusicEmitter != null)
        {
            m_SetupMenuMusicEmitter.Stop();
            m_SetupMenuMusicEmitter = null;
        }
    }
}
