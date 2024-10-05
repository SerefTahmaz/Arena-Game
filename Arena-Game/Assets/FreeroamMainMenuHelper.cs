using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame;
using UnityEngine;

public class FreeroamMainMenuHelper : MonoBehaviour
{
    [SerializeField] private cButton m_Button;

    private void Awake()
    {
        m_Button.OnClickEvent.AddListener(HandleOnClicked);
    }
 
    private void HandleOnClicked()
    {
        Main.Instance.UnityTransport.SetConnectionData("127.0.0.1", 7777);
        cGameManager.Instance.CurrentGameMode = eGameMode.Freeroam;
        cRelayManager.Instance.StartSinglePlayer();
    }
}
