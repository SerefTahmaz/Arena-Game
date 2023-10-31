using System;
using System.Collections;
using DG.Tweening;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class cPlayerCharacterNetworkController:cCharacterNetworkController
{
    [SerializeField] private cPlayerCharacter m_PlayerCharacter;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        if (IsOwner)
        {
            PlayerName.Value = cLobbyManager.Instance.PlayerName;
        }
        m_PlayerCharacter.HealthBar.InitHealthBar(PlayerName.Value.Value, m_PlayerCharacter.StartHealth);
        m_PlayerCharacter.HealthBar.SetVisibilty(true);

        PlayerName.OnValueChanged += (value, newValue) =>
        {
            m_PlayerCharacter.HealthBar.UpdateName(newValue.Value);
        };
    }

    // [ServerRpc(RequireOwnership = false)]
    // public void InitializeServerRpc(string clientName)
    // {
    //     InitializeClientRpc(clientName);
    // }
    
    // [ClientRpc]
    // public void InitializeClientRpc(string clientName)
    // {
    //     m_PlayerCharacter.CharacterName = clientName;
    //     m_PlayerCharacter.HealthBar.InitHealthBar(m_PlayerCharacter.CharacterName, m_PlayerCharacter.StartHealth);
    //     m_PlayerCharacter.HealthBar.SetVisibilty(true);
    // }

    public void TestOwnerShip()
    {
        Debug.Log(IsOwner);
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc()
    {
        TakeDamageClientRpc();
    }

    [ClientRpc]
    public void TakeDamageClientRpc()
    {
        m_PlayerCharacter.PlayerStateMachineV2.OnDamageAnim();
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void OnDeathServerRpc()
    {
        OnDeathClientRpc();
    }

    [ClientRpc]
    public void OnDeathClientRpc()
    {
        DOVirtual.DelayedCall(2, () => m_PlayerCharacter.HealthBar.SetVisibilty(false));
        m_PlayerCharacter.SoundEffectController.PlayDead();
        m_PlayerCharacter.SoundEffectController.PlayDamageGrunt();
    }
}



#if UNITY_EDITOR
[CustomEditor(typeof(cPlayerCharacterNetworkController))]
public class cPlayerCharacterNetworkControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Click"))
        {
            (target as cPlayerCharacterNetworkController).TestOwnerShip();
        }
    }
}
#endif