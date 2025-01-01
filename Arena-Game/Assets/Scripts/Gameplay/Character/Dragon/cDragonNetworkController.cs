using DG.Tweening;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class cDragonNetworkController : cCharacterNetworkController
{
    [SerializeField] private cDragonCharacter m_DragonCharacter;
    
    protected override cCharacter m_Character => m_DragonCharacter;

    [ServerRpc]
    public void OnStartFightServerRpc()
    {
        OnStartFightClientRpc();
    }
    
    [ClientRpc]
    public void OnStartFightClientRpc()
    {
        m_DragonCharacter.HealthManager.HealthBarState =
            m_DragonCharacter.CharacterNetworkController.HealthBarState.Value;
        m_DragonCharacter.HealthManager.SetVisibility(true);
        m_DragonCharacter.DragonSoundController.PlayBossMusicClientRpc();
        m_DragonCharacter.DragonSoundController.StopAmbient();
    }

    [ServerRpc(RequireOwnership = false)]
    public override void OnDeathServerRpc()
    {
        OnDeathClientRpc();
    }

    [ClientRpc]
    protected override void OnDeathClientRpc()
    {
        DOVirtual.DelayedCall(2, () => m_DragonCharacter.HealthManager.SetVisibility(false));
        m_DragonCharacter.DragonSoundController.StopBossMusic();
    }
}