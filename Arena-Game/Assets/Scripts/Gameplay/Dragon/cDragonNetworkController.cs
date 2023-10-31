using DG.Tweening;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class cDragonNetworkController : cCharacterNetworkController
{
    [SerializeField] private cDragonCharacter m_DragonCharacter;

    [ServerRpc]
    public void OnStartFightServerRpc()
    {
        OnStartFightClientRpc();
    }
    
    [ClientRpc]
    public void OnStartFightClientRpc()
    {
        m_DragonCharacter.HealthBar.InitHealthBar(m_DragonCharacter.CharacterName, m_DragonCharacter.StartHealth);
        m_DragonCharacter.HealthBar.SetVisibilty(true);
        m_DragonCharacter.DragonSoundController.PlayBossMusicClientRpc();
        m_DragonCharacter.DragonSoundController.StopAmbient();
    }

    [ServerRpc(RequireOwnership = false)]
    public override void TakeDamageServerRpc(Vector3 pos)
    {
        TakeDamageClientRpc(pos);
    }

    [ClientRpc]
    protected override void TakeDamageClientRpc(Vector3 pos)
    {
        m_DragonCharacter.DragonDamageController.DamageAnim(pos);
    }

    [ServerRpc(RequireOwnership = false)]
    public override void OnDeathServerRpc()
    {
        OnDeathClientRpc();
    }

    [ClientRpc]
    protected override void OnDeathClientRpc()
    {
        DOVirtual.DelayedCall(2, () => m_DragonCharacter.HealthBar.SetVisibilty(false));
        m_DragonCharacter.DragonSoundController.StopBossMusic();
    }
}