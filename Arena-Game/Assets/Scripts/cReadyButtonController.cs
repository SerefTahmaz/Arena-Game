using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cReadyButtonController : MonoBehaviour
{
    [SerializeField] private Image m_Image;
    
    private bool m_isReady = false;

    public void OnClick()
    {
        m_isReady = !m_isReady;
        UpdateReadyState();
    }
    
    private void UpdateReadyState()
    {
        cLobbyManager.Instance.UpdateIsPlayerReady(m_isReady);

        if (m_isReady)
        {
            m_Image.color = Color.green;
        }
        else
        {
            m_Image.color = Color.gray;
        }
    }
}
