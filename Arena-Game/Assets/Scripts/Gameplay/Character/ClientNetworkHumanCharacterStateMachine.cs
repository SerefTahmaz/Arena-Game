using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Managers.SaveManager;
using DefaultNamespace;
using Gameplay.Item;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class ClientNetworkHumanCharacterStateMachine : NetworkBehaviour
{
    [SerializeField] private HumanCharacterStateMachine m_HumanCharacterSM;

    private NetworkVariable<bool> m_IsLeftSwordDrawn = 
        new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<bool> m_IsRightSwordDrawn = 
        new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<bool> m_IsLeftSwordCharged = 
        new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<bool> m_IsRightSwordCharged = 
        new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            m_HumanCharacterSM.OnSwitchLeftWeapon += SwitchWeapon;
            m_HumanCharacterSM.OnSwitchRightWeapon += SwitchWeapon;
            m_HumanCharacterSM.OnSetLeftFlame += SetFlame;
            m_HumanCharacterSM.OnSetRightFlame += SetFlame;
            m_HumanCharacterSM.Init();
        }
        else
        {
            WeaponValueChanged(false,m_IsLeftSwordDrawn.Value);
            WeaponValueChanged(false,m_IsRightSwordDrawn.Value);
            SwordChargedValueChanged(false,m_IsLeftSwordCharged.Value);
            SwordChargedValueChanged(false, m_IsRightSwordCharged.Value);

            m_IsLeftSwordDrawn.OnValueChanged += WeaponValueChanged;
            m_IsRightSwordDrawn.OnValueChanged += WeaponValueChanged;
            m_IsLeftSwordCharged.OnValueChanged += SwordChargedValueChanged;
            m_IsRightSwordCharged.OnValueChanged += SwordChargedValueChanged;
        }
    }

    private void SwordChargedValueChanged(bool previousvalue, bool newvalue)
    {
        m_HumanCharacterSM.IsLeftSwordCharged = m_IsLeftSwordCharged.Value;
        m_HumanCharacterSM.SetLeftFlameVisuals(m_IsLeftSwordCharged.Value);

        m_HumanCharacterSM.IsRightSwordCharged = m_IsRightSwordCharged.Value;
        m_HumanCharacterSM.SetRightFlameVisuals(m_IsRightSwordCharged.Value);
    }

    private void SetFlame()
    {
        m_IsLeftSwordCharged.Value = m_HumanCharacterSM.IsLeftSwordCharged;
        m_IsRightSwordCharged.Value = m_HumanCharacterSM.IsRightSwordCharged;
    }

    private void WeaponValueChanged(bool previousvalue, bool newvalue)
    {
        m_HumanCharacterSM.SetLeftWeaponVisuals(m_IsLeftSwordDrawn.Value);
        m_HumanCharacterSM.IsLeftSwordDrawn = m_IsLeftSwordDrawn.Value;
        
        m_HumanCharacterSM.SetRightWeaponVisuals(m_IsRightSwordDrawn.Value);
        m_HumanCharacterSM.IsRightSwordDrawn = m_IsRightSwordDrawn.Value;
    }

    private void SwitchWeapon()
    {
        m_IsLeftSwordDrawn.Value = m_HumanCharacterSM.IsLeftSwordDrawn;
        m_IsRightSwordDrawn.Value = m_HumanCharacterSM.IsRightSwordDrawn;
    }

   
    private void Update()
    {
        if(!IsOwner) return;
    }
}
