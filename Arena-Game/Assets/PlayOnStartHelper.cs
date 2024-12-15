using System.Collections;
using System.Collections.Generic;
using ArenaGame.UI;
using AudioSystem;
using UnityEngine;

public class PlayOnStartHelper : MonoBehaviour
{
    [SerializeField] private AudioClip m_AudioClip;
    [SerializeField] private float m_Volume;
    [SerializeField] private cView m_View;
    
    void Awake()
    {
        m_View.OnActivateEvent.AddListener((() =>
        {
            SoundManager.PlayOneShot2D(m_AudioClip,m_Volume);
        }));
    }
}
