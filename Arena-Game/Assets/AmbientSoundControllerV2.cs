using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArenaGame.Utils;
using AudioSystem;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;

public class AmbientSoundControllerV2 : MonoBehaviour
{
    [SerializeField] private SoundsHolder m_SoundsHolder;
    [SerializeField] private PlayerDetector m_PlayerDetector;
    [SerializeField] private SoundData m_SoundData;

    private SoundEmitter m_SoundEmitter;
    private int m_PlayerEnteranceCount;
    private AudioClip m_ClipToPlay;

    private List<Collider> m_Cols;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Cols = GetComponentsInChildren<Collider>().ToList();
        
        if (m_SoundsHolder != null)
        {
            m_ClipToPlay = m_SoundsHolder.AudioClips.RandomItem();
            m_SoundData.clip = m_ClipToPlay;
        }
        SoundBuilder soundBuilder = SoundManager.Instance.CreateSoundBuilder();
                
        var emitter = soundBuilder
            .WithPosition(transform.position)
            .WithParent(transform)
            .Play(m_SoundData);
        m_SoundEmitter = emitter;
        
        m_PlayerDetector.OnPlayerEntered += OnPlayerEntered;
        m_PlayerDetector.OnPlayerExit += OnPlayerExit;
    }

    private void Update()
    {
        if(GameplayStatics.OwnerPlayer == null) return;
        if (m_PlayerEnteranceCount > 0)
        {
            m_SoundEmitter.transform.position = GameplayStatics.OwnerPlayer.position;
        }
        else
        {
            
            var closestPos = m_Cols.Select((collider1 => collider1.ClosestPoint(GameplayStatics.OwnerPlayer.position))).OrderBy((
                vector3 => Vector3.Distance(GameplayStatics.OwnerPlayer.position, vector3))).FirstOrDefault();
            
            m_SoundEmitter.transform.position = closestPos;
        }
    }

    private void OnPlayerEntered()
    {
        m_PlayerEnteranceCount++;
    }
    
    private void OnPlayerExit()
    {
        m_PlayerEnteranceCount--;
    }
}