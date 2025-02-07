using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using DefaultNamespace;
using UnityEngine;

public class InternetManager : cSingleton<InternetManager>
{
    private bool m_NoInternet;
    private float m_NoInternetDuration;
    private bool m_CheckInternetConnection;

    // Update is called once per frame
    void Update()
    {
        if(!m_CheckInternetConnection) return;
        
        m_NoInternet = !GameplayStatics.CheckInternetConnection();

        if (m_NoInternet)
        {
            m_NoInternetDuration += Time.deltaTime;
        }
        else
        {
            m_NoInternetDuration = 0;
        }

        if (m_NoInternetDuration > 5)
        {
            cGameManager.Instance.HandleNoInternet();
            SetCheckInternetConnection(false);
        }
        
        // Debug.Log($"No internet duration: {m_NoInternetDuration}");
    }
    
    public void SetCheckInternetConnection(bool value)
    {
        m_CheckInternetConnection = value;
        m_NoInternetDuration = 0;
    }
}