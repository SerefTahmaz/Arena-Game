using System;
using AudioSystem;
using UnityEngine;

public class MainMenuMusicHelper : MonoBehaviour
{
    [SerializeField] private AudioClip m_MainMenuMusicClip;
    [SerializeField] private float m_Volume;
        
    private SoundEmitter m_MainMenuMusicEmitter;
        
    private void Awake()
    {
        cGameManager.Instance.OnMainMenuEnter += MainMenuEnter;
        cGameManager.Instance.OnMainMenuExit += MainMenuExit;
        // MainMenuEnter();
    }
        
    private void OnDestroy()
    {
        if (cGameManager.Instance != null)
        {
            cGameManager.Instance.OnMainMenuEnter -= MainMenuEnter;
            cGameManager.Instance.OnMainMenuExit -= MainMenuExit;
        }
    }
        
    private void MainMenuExit()
    {
        if (m_MainMenuMusicEmitter != null)
        {
            m_MainMenuMusicEmitter.Stop();
            m_MainMenuMusicEmitter = null;
        }
    }

    private void MainMenuEnter()
    {
        m_MainMenuMusicEmitter = SoundManager.PlayOneShot2D(m_MainMenuMusicClip, m_Volume);
    }
}