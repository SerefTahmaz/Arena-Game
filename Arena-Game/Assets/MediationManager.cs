using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using UnityEngine;

public class MediationManager : cSingleton<MediationManager>
{
    [SerializeField] private LevelPlayManager m_LevelPlayManager;

    public IMediationService MediationService => m_LevelPlayManager;

    private void Start()
    {
        m_LevelPlayManager.Init();
    }
}

public interface IMediationService
{
    public void Init();
    public void LoadBanner();
    public void LoadInterstitial();
    public bool ShowInterstitial();
    public void ShowRewarded();
    public Action OnInterstitialEnded { get; set; }
    public Action OnInterstitialStarted { get; set; }
}