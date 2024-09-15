using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame;
using DG.Tweening;
using Unity.Networking.Transport.Relay;
using UnityEngine;

public class PvPSingleMainMenuHelper : MonoBehaviour
{
    private void Awake()
    {
        transform.DOScale(0.05f, 0.8f).SetLoops(-1, LoopType.Yoyo).SetRelative(true);
    }

    public void OnClick()
    {
        Main.Instance.UnityTransport.SetConnectionData("127.0.0.1", 7777);
        cGameManager.Instance.CurrentGameMode = eGameMode.PvPSingle;
        cRelayManager.Instance.StartPvPSingle();
    }
}
