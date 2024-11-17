using System;
using System.Collections;
using ArenaGame;
using DefaultNamespace;
using DG.Tweening;
using Gameplay.Character;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class cPlayerCharacterNetworkController:cCharacterNetworkController
{
    [SerializeField] private HumanCharacter m_HumanCharacter;
    
    protected override cCharacter m_Character => m_HumanCharacter;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        if (IsOwner)
        {
            PlayerName.Value = ProfileGenerator.GetPlayerProfile().Name;
        }

        m_HumanCharacter.CharacterName = PlayerName.Value.Value;
        m_HumanCharacter.HealthManager.SetVisibility(true);

        PlayerName.OnValueChanged += (value, newValue) =>
        {
            m_HumanCharacter.HealthManager.UpdateUIClientRpc();
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
        m_HumanCharacter.OnDamageAnim();
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
        DOVirtual.DelayedCall(2, () =>
        {
            m_HumanCharacter.HealthManager.SetVisibility(false);
        });
        m_HumanCharacter.SoundEffectController.PlayDead();
        m_HumanCharacter.SoundEffectController.PlayDamageGrunt();
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