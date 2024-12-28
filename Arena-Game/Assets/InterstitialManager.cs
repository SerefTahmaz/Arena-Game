using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Managers.SaveManager;
using UnityEngine;

public class InterstitialManager : MonoBehaviour
{
    [SerializeField] private float m_InterInterval=120;
    [SerializeField] private float m_FirstTimeInterInterval = 1200;
    [SerializeField] private int m_IntervalMult;
    
    private float m_ShowTimer;
    private bool m_InterShowing;
    private bool m_PendingShowingInter;
    private float m_FirstTimeDuration;
    
    // Start is called before the first frame update
    void Start()
    {
        MediationManager.Instance.MediationService.OnInterstitialStarted += HandleInterstitialStarted;
        MediationManager.Instance.MediationService.OnInterstitialEnded += HandleInterstitialEnded;
        cGameManager.Instance.m_GameEnded += HandleGameEnded;

        var saveData = UtilitySaveHandler.SaveData;
        if (saveData.m_InterstitialShownCount == 0)
        {
            m_InterInterval = m_FirstTimeInterInterval;
        }
        else
        {
            //First five level higher interval
            var ftueMul = 6 - saveData.m_InterstitialShownCount;
            ftueMul = Mathf.Max(1, ftueMul);
            m_InterInterval *= ftueMul;

            m_IntervalMult = ftueMul;
        }
        
        saveData.m_InterstitialShownCount++;
        UtilitySaveHandler.Save();
    }

    private void OnDestroy()
    {
        if(MediationManager.Instance) MediationManager.Instance.MediationService.OnInterstitialStarted -= HandleInterstitialStarted;
        if(MediationManager.Instance) MediationManager.Instance.MediationService.OnInterstitialEnded -= HandleInterstitialEnded;
        if(cGameManager.Instance) cGameManager.Instance.m_GameEnded -= HandleGameEnded;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_InterShowing || m_PendingShowingInter) return;
        
        m_ShowTimer += Time.deltaTime;
        if (m_ShowTimer >= m_InterInterval)
        {
            m_ShowTimer = 0;
            m_PendingShowingInter = true;
        }
    }
    
    private void HandleGameEnded()
    {
        ShowInterstitial();
    }

    private void ShowInterstitial()
    {
        if (m_PendingShowingInter)
        {
            MediationManager.Instance.MediationService.ShowInterstitial();
            m_PendingShowingInter = false;
        }
    }
    
    private void HandleInterstitialStarted()
    {
        m_InterShowing = true;
    }
    
    private void HandleInterstitialEnded()
    {
        m_InterShowing = false;
    }
}
