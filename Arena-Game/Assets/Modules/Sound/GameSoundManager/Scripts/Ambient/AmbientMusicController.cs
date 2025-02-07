using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArenaGame.Managers.SaveManager;
using ArenaGame.Utils;
using AudioSystem;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class AmbientMusicController : MonoBehaviour
{
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private List<AmbientMusic> m_AmbientMusics;
    
    [Serializable]
    public class AmbientMusic
    {
        public float FadeInDuration;
        public AudioClip Clip;
    }

    private Tween m_FadeInTween;

    private List<AmbientMusic> m_PlayedList = new List<AmbientMusic>();
    private bool m_GameEnded;
    
    private void Awake()
    {
        MultiplayerLocalHelper.Instance.OnMultiplayerGameStarted += HandleOnGameStarted;
        cGameManager.Instance.m_GameEnded += HandleGameEnded;
    }
    
    private void OnDestroy()
    {
        if (MultiplayerLocalHelper.Instance)
        {
            MultiplayerLocalHelper.Instance.OnMultiplayerGameStarted -= HandleOnGameStarted;
        }

        if (cGameManager.Instance)
        {
            cGameManager.Instance.m_GameEnded -= HandleGameEnded;
        }
    }
    
    private void HandleOnGameStarted()
    {
        if (UtilitySaveHandler.SaveData.m_FreeroamPlayCount < 1)
        { 
            Play(m_AmbientMusics[0]);
        }
        else
        {
            Play(m_AmbientMusics.RandomItem());
        }

        UtilitySaveHandler.SaveData.m_FreeroamPlayCount++;
        UtilitySaveHandler.Save();
    }
    
    private void HandleGameEnded()
    {
        m_AudioSource.Stop();
        m_GameEnded = true;
    }
    
    public async UniTask Play(AmbientMusic ambientMusic)
    {
        m_PlayedList.Add(ambientMusic);
        
        m_AudioSource.Stop();
        m_AudioSource.clip = ambientMusic.Clip;
        m_AudioSource.Play();

        await UniTask.WaitUntil((() => !m_AudioSource.isPlaying), cancellationToken: this.GetCancellationTokenOnDestroy());
        if(m_GameEnded) return;
        await UniTask.WaitForSeconds(Random.Range(5, 90), cancellationToken: this.GetCancellationTokenOnDestroy());
        if(m_GameEnded) return;


        var ambientMusics = m_AmbientMusics.Except(m_PlayedList);
        if (!ambientMusics.Any())
        {
            m_PlayedList.Clear();
            ambientMusics = m_AmbientMusics.Except(m_PlayedList);
        }
        
        var randomItem = ambientMusics.RandomItem();
        Play(randomItem);
    }
}