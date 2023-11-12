using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using UnityEngine;

public class cReturnToLobbyButton : MonoBehaviour
{
    public void ReturnToLobby()
    {
        cLobbyUI.Instance.DisableLobbyUI();
        cLobbyListUI.Instance.EnableLobbyListUI();
        cLobbyManager.Instance.KickPlayer(AuthenticationService.Instance.PlayerId);
    }
}
