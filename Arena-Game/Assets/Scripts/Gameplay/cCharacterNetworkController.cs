using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class cCharacterNetworkController : NetworkBehaviour
{
    [SerializeField] private NetworkAnimator m_NetworkAnimator;

    public NetworkAnimator NetworkAnimator => m_NetworkAnimator;

    public NetworkVariable<float> CurrentHealth
    {
        get => m_CurrentHealth;
        set => m_CurrentHealth = value;
    }

    public NetworkVariable<FixedString128Bytes> PlayerName
    {
        get => m_PlayerName;
        set => m_PlayerName = value;
    }

    private NetworkVariable<float> m_CurrentHealth = new NetworkVariable<float>(100,NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    
    private NetworkVariable<FixedString128Bytes> m_PlayerName = new NetworkVariable<FixedString128Bytes>("Player",NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    private void Update()
    {
        if(!IsOwner) return;
    }
}
