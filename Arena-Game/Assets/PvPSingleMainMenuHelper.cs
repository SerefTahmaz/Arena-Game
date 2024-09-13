using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PvPSingleMainMenuHelper : MonoBehaviour
{
    public void OnClick()
    {
        cGameManager.Instance.CurrentGameMode = eGameMode.PvPSingle;
        cRelayManager.Instance.StartPvPSingle();
    }
}
