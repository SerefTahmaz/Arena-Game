using System.Collections;
using System.Collections.Generic;
using ArenaGame;
using Unity.Networking.Transport.Relay;
using UnityEngine;

public class PvPSingleMainMenuHelper : MonoBehaviour
{
    public void OnClick()
    {
        Main.Instance.UnityTransport.SetConnectionData("127.0.0.1", 7777);
        cGameManager.Instance.CurrentGameMode = eGameMode.PvPSingle;
        cRelayManager.Instance.StartPvPSingle();
    }
}
