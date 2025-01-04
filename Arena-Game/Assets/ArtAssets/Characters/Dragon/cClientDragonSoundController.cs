using Unity.Netcode;
using UnityEngine;

namespace ArtAssets.Characters.Dragon
{
    public class cClientDragonSoundController : NetworkBehaviour
    {
        [SerializeField] private cDragonSoundController m_DragonSoundController;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsHost)
            {
                m_DragonSoundController.OnPlayShout += OnPlayShoutClientRpc;
                m_DragonSoundController.OnPlayAttack2 += OnPlayAttack2ClientRpc;
                m_DragonSoundController.OnPlayAttack1 += OnPlayAttack1ClientRpc;
                m_DragonSoundController.OnPlayFlyBreathingClip += OnPlayFlyBreathingClipClientRpc;
                m_DragonSoundController.OnPlayMeleeAttackClip += OnPlayMeleeAttackClipClientRpc;
                m_DragonSoundController.OnPlayMeleeAttack2Clip += OnPlayMeleeAttack2ClipClientRpc;
                m_DragonSoundController.OnPlayDeathClip += OnPlayDeathClipClientRpc;
                m_DragonSoundController.OnPlayFlyIdleClip += OnPlayFlyIdleClipClientRpc;
                m_DragonSoundController.OnPlayTransitionToFlyClip += OnPlayTransitionToFlyClipClientRpc;
                m_DragonSoundController.OnPlayFlyToGroundClip += OnPlayFlyToGroundClipClientRpc;
                m_DragonSoundController.OnPlayTurn360Clip += OnPlayTurn360ClipClientRpc;
                m_DragonSoundController.OnPlayForwardJumpClip += OnPlayForwardJumpClipClientRpc;
                
                m_DragonSoundController.Init();
            }
            else
            {
                
            }
        }
        
        [ClientRpc]
        private void OnPlayShoutClientRpc()
        {
            m_DragonSoundController.PlayShoutAction();
        }
        
        [ClientRpc]
        private void OnPlayAttack2ClientRpc()
        {
            m_DragonSoundController.PlayAttack2Action();
        }
        
        [ClientRpc]
        private void OnPlayAttack1ClientRpc()
        {
            m_DragonSoundController.PlayAttack1Action();
        }
        
        [ClientRpc]
        private void OnPlayFlyBreathingClipClientRpc()
        {
            m_DragonSoundController.PlayFlyBreathingClipAction();
        }
        
        [ClientRpc]
        private void OnPlayMeleeAttackClipClientRpc()
        {
            m_DragonSoundController.PlayMeleeAttackClipAction();
        }
        
        [ClientRpc]
        private void OnPlayMeleeAttack2ClipClientRpc()
        {
            m_DragonSoundController.PlayMeleeAttack2ClipAction();
        }
        
        [ClientRpc]
        private void OnPlayDeathClipClientRpc()
        {
            m_DragonSoundController.PlayDeathClipAction();
        }
        
        [ClientRpc]
        private void OnPlayFlyIdleClipClientRpc()
        {
            m_DragonSoundController.PlayFlyIdleClipAction();
        }
        
        [ClientRpc]
        private void OnPlayTransitionToFlyClipClientRpc()
        {
            m_DragonSoundController.PlayTransitionToFlyClipAction();
        }
        
        [ClientRpc]
        private void OnPlayFlyToGroundClipClientRpc()
        {
            m_DragonSoundController.PlayFlyToGroundClipAction();
        }
        
        [ClientRpc]
        private void OnPlayTurn360ClipClientRpc()
        {
            m_DragonSoundController.PlayTurn360ClipAction();
        }
        
        [ClientRpc]
        private void OnPlayForwardJumpClipClientRpc()
        {
            m_DragonSoundController.PlayForwardJumpClipAction();
        }
    }
}