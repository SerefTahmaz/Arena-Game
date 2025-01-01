using DG.Tweening;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class cTrollNetworkController : cCharacterNetworkController
{
    [SerializeField] private cTrollCharacter m_TrollCharacter;
    
    protected override cCharacter m_Character => m_TrollCharacter;

    [ServerRpc]
    public void OnStartFightServerRpc()
    {
        OnStartFightClientRpc();
    }
    
    [ClientRpc]
    public void OnStartFightClientRpc()
    {
        // Debug.Log($"Creature healthbar state {m_TrollCharacter.CharacterNetworkController.HealthBarState.Value}");
        UpdateHealthBar(cHealthManager.eHealthBarState.World, m_TrollCharacter.CharacterNetworkController.HealthBarState.Value);

        m_TrollCharacter.CharacterNetworkController.HealthBarState.OnValueChanged += UpdateHealthBar;
    }

    private void UpdateHealthBar(cHealthManager.eHealthBarState previousvalue, cHealthManager.eHealthBarState newvalue)
    {
        m_TrollCharacter.HealthManager.DisableHealthBar();
        m_TrollCharacter.HealthManager.HealthBarState =
            m_TrollCharacter.CharacterNetworkController.HealthBarState.Value;
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