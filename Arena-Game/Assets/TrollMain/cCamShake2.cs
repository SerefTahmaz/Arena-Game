using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;


public class cCamShake2 : MonoBehaviour
{
    private CinemachineVirtualCamera m_CinemachineVirtualCamera;
    private float m_ShakerTimer;
    private float m_ShakeTimerTotal;
    private float m_StartingIntensity;
    private Camera m_MainCam;
    
    private void Start()
    {
        m_CinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_ShakerTimer > 0)
        {
            m_ShakerTimer -= Time.unscaledDeltaTime;
            var cinemachineBasicMultiChannelPerlin = 
                m_CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain =
                Mathf.Lerp(m_StartingIntensity, 0, 1 - (m_ShakerTimer / m_ShakeTimerTotal));
        }
    }

    public void ShakeCamera(float intensity, float freq, float time)
    {
        if (m_CinemachineVirtualCamera == null)
        {
            m_CinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        }
        
        var cinemachineBasicMultiChannelPerlin = 
            m_CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = freq;
        m_StartingIntensity = intensity;
        m_ShakerTimer = time;
        m_ShakeTimerTotal = time;
    }
}