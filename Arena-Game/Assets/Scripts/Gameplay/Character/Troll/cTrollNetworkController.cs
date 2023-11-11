using DG.Tweening;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class cTrollNetworkController : cCharacterNetworkController
{
    [SerializeField] private cTrollCharacter m_TrollCharacter;

    [ServerRpc]
    public void OnStartFightServerRpc()
    {
        OnStartFightClientRpc();
    }
    
    [ClientRpc]
    public void OnStartFightClientRpc()
    {
        m_TrollCharacter.HealthManager.SetVisibility(true);
    }
    
    [ServerRpc(RequireOwnership = false)]
    public override void TakeDamageServerRpc(Vector3 pos)
    {
        TakeDamageClientRpc(pos);
    }

    [ClientRpc]
    protected override void TakeDamageClientRpc(Vector3 pos)
    {
        m_TrollCharacter.TrollStateMachine.OnDamageAnim(pos);
    }

    [ServerRpc(RequireOwnership = false)]
    public override void OnDeathServerRpc()
    {
        OnDeathClientRpc();
    }

    [ClientRpc]
    protected override void OnDeathClientRpc()
    {
        DOVirtual.DelayedCall(2, () => m_TrollCharacter.HealthManager.SetVisibility(false));
    }
}