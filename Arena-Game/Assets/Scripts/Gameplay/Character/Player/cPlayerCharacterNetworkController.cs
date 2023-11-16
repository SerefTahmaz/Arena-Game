using System;
using System.Collections;
using DefaultNamespace;
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
    
    protected override cCharacter m_Character => m_PlayerCharacter;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        if (IsOwner)
        {
            PlayerName.Value = cLobbyManager.Instance.PlayerName;
        }

        m_PlayerCharacter.CharacterName = PlayerName.Value.Value;
        m_PlayerCharacter.HealthManager.SetVisibility(true);

        PlayerName.OnValueChanged += (value, newValue) =>
        {
            m_PlayerCharacter.HealthManager.UpdateUIClientRpc();
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
    public override void TakeDamageServerRpc(Vector3 pos)
    {
        TakeDamageClientRpc(pos);
    }

    [ClientRpc]
    protected override void TakeDamageClientRpc(Vector3 pos)
    {
        m_PlayerCharacter.PlayerStateMachineV2.OnDamageAnim();
    }
    
    [ServerRpc(RequireOwnership = false)]
    public override void OnDeathServerRpc()
    {
        OnDeathClientRpc();
        cGameManager.Instance.m_OnPlayerDied.Invoke();
    }
    
    [ClientRpc]
    protected override void OnDeathClientRpc()
    {
        DOVirtual.DelayedCall(2, () => m_PlayerCharacter.HealthManager.SetVisibility(false));
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