using System;
using UnityEngine;
using System.Collections.Generic;
using com.unity3d.mediation;

public class LevelPlayManager : MonoBehaviour,IMediationService
{
    [SerializeField] private GameObject m_FakeInterstitial;
    
    private LevelPlayBannerAd bannerAd;
    private LevelPlayInterstitialAd interstitialAd;
     
#if UNITY_ANDROID
    string appKey = "2093845ed";
    string bannerAdUnitId = "7jt21gh77fp05j2o";
    string interstitialAdUnitId = "8z7b97pun7wlzljw";
#elif UNITY_IPHONE
    string appKey = "209387f75";
    string bannerAdUnitId = "1fszl65lfjtushu9";
    string interstitialAdUnitId = "8fh8xjr9ksmdmrzv";
#else
    string appKey = "unexpected_platform";
    string bannerAdUnitId = "unexpected_platform";
    string interstitialAdUnitId = "unexpected_platform";
#endif

    public Action OnInterstitialEnded { get; set; }
    public Action OnInterstitialStarted { get; set; }

    public void Init()
    {
        Debug.Log("unity-script: IronSource.Agent.validateIntegration");
        IronSource.Agent.validateIntegration();

        Debug.Log("unity-script: unity version" + IronSource.unityVersion());

        // SDK init
        Debug.Log("unity-script: LevelPlay SDK initialization");
        LevelPlay.Init(appKey,adFormats:new []{LevelPlayAdFormat.REWARDED});
        
        LevelPlay.OnInitSuccess += SdkInitializationCompletedEvent;
        LevelPlay.OnInitFailed += SdkInitializationFailedEvent;
    }

    void EnableAds()
    {
        //Add ImpressionSuccess Event
        IronSourceEvents.onImpressionDataReadyEvent += ImpressionDataReadyEvent;

        //Add AdInfo Rewarded Video Events
        IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;
        
        bannerAd = new LevelPlayBannerAd(bannerAdUnitId);

        // Register to Banner events
        bannerAd.OnAdLoaded += BannerOnAdLoadedEvent;
        bannerAd.OnAdLoadFailed += BannerOnAdLoadFailedEvent;
        bannerAd.OnAdDisplayed += BannerOnAdDisplayedEvent;
        bannerAd.OnAdDisplayFailed += BannerOnAdDisplayFailedEvent;
        bannerAd.OnAdClicked += BannerOnAdClickedEvent;
        bannerAd.OnAdCollapsed += BannerOnAdCollapsedEvent;
        bannerAd.OnAdLeftApplication += BannerOnAdLeftApplicationEvent;
        bannerAd.OnAdExpanded += BannerOnAdExpandedEvent;

        // Create Interstitial object
        interstitialAd = new LevelPlayInterstitialAd(interstitialAdUnitId);
        
        // Register to Interstitial events
        interstitialAd.OnAdLoaded += InterstitialOnAdLoadedEvent;
        interstitialAd.OnAdLoadFailed += InterstitialOnAdLoadFailedEvent;
        interstitialAd.OnAdDisplayed += InterstitialOnAdDisplayedEvent;
        interstitialAd.OnAdDisplayFailed += InterstitialOnAdDisplayFailedEvent;
        interstitialAd.OnAdClicked += InterstitialOnAdClickedEvent;
        interstitialAd.OnAdClosed += InterstitialOnAdClosedEvent;
        interstitialAd.OnAdInfoChanged += InterstitialOnAdInfoChangedEvent;
        
        LoadInterstitial();
    }

    void OnApplicationPause(bool isPaused)
    {
        Debug.Log("OnApplicationPause = " + isPaused);
        IronSource.Agent.onApplicationPause(isPaused);
    }

    public void ShowRewarded()
    {
        Debug.Log("ShowRewardedVideo");
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            IronSource.Agent.showRewardedVideo();
        }
        else
        {
            Debug.Log("isRewardedVideoAvailable - False");
        }
    }

    public void LoadInterstitial()
    {
        Debug.Log("Load Interstitial");
        interstitialAd.LoadAd();
    }
    
    public bool ShowInterstitial()
    {
       
#if UNITY_EDITOR
        Instantiate(m_FakeInterstitial);
        return true;
#endif
        
        Debug.Log("Show Interstitial");
        if (interstitialAd.IsAdReady())
        {
            interstitialAd.ShowAd();
            return true;
        }
        else
        {
            Debug.Log("Levelplay Interstital Ad Ready? - False");
            LoadInterstitial();
            return false;
        }
    }

    public void LoadBanner()
    {
        Debug.Log("Load Banner");
        bannerAd.LoadAd();
    }

    public void HideBanner()
    {
        Debug.Log("Hide Banner");
        bannerAd.HideAd();
    }

    #region Init callback handlers

    void SdkInitializationCompletedEvent(LevelPlayConfiguration config)
    {
        Debug.Log("unity-script: I got SdkInitializationCompletedEvent with config: "+ config);
        EnableAds();
    }
    
    void SdkInitializationFailedEvent(LevelPlayInitError error)
    {
        Debug.Log("unity-script: I got SdkInitializationFailedEvent with error: "+ error);
    }

    #endregion

    #region AdInfo Rewarded Video
    void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got RewardedVideoOnAdOpenedEvent With AdInfo " + adInfo);
    }

    void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got RewardedVideoOnAdClosedEvent With AdInfo " + adInfo);
    }

    void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got RewardedVideoOnAdAvailable With AdInfo " + adInfo);
    }

    void RewardedVideoOnAdUnavailable()
    {
        Debug.Log("unity-script: I got RewardedVideoOnAdUnavailable");
    }

    void RewardedVideoOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got RewardedVideoOnAdShowFailedEvent With Error" + ironSourceError + "And AdInfo " + adInfo);
    }

    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement ironSourcePlacement, IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got RewardedVideoOnAdRewardedEvent With Placement" + ironSourcePlacement + "And AdInfo " + adInfo);
    }

    void RewardedVideoOnAdClickedEvent(IronSourcePlacement ironSourcePlacement, IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got RewardedVideoOnAdClickedEvent With Placement" + ironSourcePlacement + "And AdInfo " + adInfo);
    }

    #endregion
    #region AdInfo Interstitial

    void InterstitialOnAdLoadedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("unity-script: I got InterstitialOnAdLoadedEvent With AdInfo " + adInfo);
    }

    void InterstitialOnAdLoadFailedEvent(LevelPlayAdError error)
    {
        Debug.Log("unity-script: I got InterstitialOnAdLoadFailedEvent With Error " + error);
    }
	
    void InterstitialOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("unity-script: I got InterstitialOnAdDisplayedEvent With AdInfo " + adInfo);
        OnInterstitialStarted?.Invoke();
    }
	
    void InterstitialOnAdDisplayFailedEvent(LevelPlayAdDisplayInfoError infoError)
    {
        Debug.Log("unity-script: I got InterstitialOnAdDisplayFailedEvent With InfoError " + infoError);
        OnInterstitialEnded?.Invoke();
        LoadInterstitial();
    }
	
    void InterstitialOnAdClickedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("unity-script: I got InterstitialOnAdClickedEvent With AdInfo " + adInfo);
    }

    void InterstitialOnAdClosedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("unity-script: I got InterstitialOnAdClosedEvent With AdInfo " + adInfo);
        OnInterstitialEnded?.Invoke();
        LoadInterstitial();
    }
	
    void InterstitialOnAdInfoChangedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("unity-script: I got InterstitialOnAdInfoChangedEvent With AdInfo " + adInfo);
    }

    #endregion

    #region Banner AdInfo

    void BannerOnAdLoadedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("unity-script: I got BannerOnAdLoadedEvent With AdInfo " + adInfo);
    }

    void BannerOnAdLoadFailedEvent(LevelPlayAdError ironSourceError)
    {
        Debug.Log("unity-script: I got BannerOnAdLoadFailedEvent With Error " + ironSourceError);
    }

    void BannerOnAdClickedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("unity-script: I got BannerOnAdClickedEvent With AdInfo " + adInfo);
    }

    void BannerOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("unity-script: I got BannerOnAdDisplayedEvent With AdInfo " + adInfo);
    }
	
    void BannerOnAdDisplayFailedEvent(LevelPlayAdDisplayInfoError adInfoError)
    {
        Debug.Log("unity-script: I got BannerOnAdDisplayFailedEvent With AdInfoError " + adInfoError);
    }

    void BannerOnAdCollapsedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("unity-script: I got BannerOnAdCollapsedEvent With AdInfo " + adInfo);
    }

    void BannerOnAdLeftApplicationEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("unity-script: I got BannerOnAdLeftApplicationEvent With AdInfo " + adInfo);
    }

    void BannerOnAdExpandedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("unity-script: I got BannerOnAdExpandedEvent With AdInfo " + adInfo);
    }

    #endregion

    #region ImpressionSuccess callback handler

    void ImpressionDataReadyEvent(IronSourceImpressionData impressionData)
    {
        Debug.Log("unity - script: I got ImpressionDataReadyEvent ToString(): " + impressionData.ToString());
        Debug.Log("unity - script: I got ImpressionDataReadyEvent allData: " + impressionData.allData);
    }

    #endregion

    private void OnDisable()
    {
        bannerAd?.DestroyAd();
        interstitialAd?.DestroyAd();
    }
}