using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cCreateLobbyButton : MonoBehaviour
{
    public void CreateLobby()
    {
        cLobbyListUI.Instance.OnCreateLobby();
        PVELevelSelectView.Instance.Activate();
    }

    public void OnCreated()
    {
        Debug.Log("Created Lobby");
    }
}
