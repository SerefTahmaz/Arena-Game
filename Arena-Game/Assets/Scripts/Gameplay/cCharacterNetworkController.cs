using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public abstract class cCharacterNetworkController : NetworkBehaviour
{
    [SerializeField] private NetworkAnimator m_NetworkAnimator;

    public NetworkAnimator NetworkAnimator => m_NetworkAnimator;
    
    private Action m_OnSpawn = delegate {  };

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

    public Action OnSpawn
    {
        get => m_OnSpawn;
        set => m_OnSpawn = value;
    }

    private NetworkVariable<float> m_CurrentHealth = new NetworkVariable<float>(100,NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    
    private NetworkVariable<FixedString128Bytes> m_PlayerName = new NetworkVariable<FixedString128Bytes>("Player",NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        m_OnSpawn.Invoke();
    }

    private void Update()
    {
        if(!IsOwner) return;
    }

    [ServerRpc(RequireOwnership = false)]
    public virtual void TakeDamageServerRpc(Vector3 pos){}

    [ClientRpc]
    protected virtual void TakeDamageClientRpc(Vector3 pos){}

    [ServerRpc(RequireOwnership = false)]
    public virtual void OnDeathServerRpc(){}

    [ClientRpc]
    protected virtual void OnDeathClientRpc(){}
}
