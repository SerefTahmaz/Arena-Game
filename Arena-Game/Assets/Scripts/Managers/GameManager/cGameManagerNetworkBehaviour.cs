using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class cGameManagerNetworkBehaviour : NetworkBehaviour
{
    [SerializeField] private cButton m_MainMenuButton;
    
    [ClientRpc]
    public void OnHostLeaveClientRpc()
    {
        if (IsHost==false)
        {
            m_MainMenuButton.OnClick();
        }
    }
}
