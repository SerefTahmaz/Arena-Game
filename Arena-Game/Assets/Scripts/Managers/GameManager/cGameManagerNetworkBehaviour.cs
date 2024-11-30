using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class cGameManagerNetworkBehaviour : NetworkBehaviour
{
    [ClientRpc]
    public void OnHostLeaveClientRpc()
    {
        // if (IsHost==false)
        // {
        //     m_MainMenuButton.OnClick();
        // }
    }
}