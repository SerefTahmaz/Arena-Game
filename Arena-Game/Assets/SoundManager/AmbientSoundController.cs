using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using AudioSystem;
using DefaultNamespace;
using UnityEngine;

public class AmbientSoundController : MonoBehaviour
{
    [SerializeField] private SoundsHolder m_SoundsHolder;
    [SerializeField] private PlayerDetector m_PlayerDetector;

    private SoundEmitter m_SoundEmitter;
    
    // Start is called before the first frame update
    void Start()
    {
        m_PlayerDetector.OnPlayerEntered += OnPlayerEntered;
        m_PlayerDetector.OnPlayerExit += OnPlayerExit;
    }

    private void OnPlayerEntered()
    {
        AmbientSoundManager.Instance.PlaySound(m_SoundsHolder, this);
    }
    
    private void OnPlayerExit()
    {
        AmbientSoundManager.Instance.StopSound(m_SoundsHolder, this);
    }
}
