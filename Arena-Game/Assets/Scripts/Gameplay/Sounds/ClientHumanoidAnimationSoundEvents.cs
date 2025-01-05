using Unity.Netcode;
using UnityEngine;

namespace DefaultNamespace.SoundManager
{
     public class ClientHumanoidAnimationSoundEvents : NetworkBehaviour
    {
        [SerializeField] private HumanoidAnimationSoundEvents m_HumanoidAnimationSoundEvents;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsOwner)
            {
                m_HumanoidAnimationSoundEvents.OnRightStepEvent += OnRightStepEventServerRpc;
                m_HumanoidAnimationSoundEvents.OnLeftStepEvent += OnLeftStepEventServerRpc;
                m_HumanoidAnimationSoundEvents.OnPlaySwordDraw += OnPlaySwordDrawServerRpc;
                m_HumanoidAnimationSoundEvents.OnPlayDSlash += OnPlayDSlashServerRpc;
                m_HumanoidAnimationSoundEvents.OnPlayFireCharge += OnPlayFireChargeServerRpc;
                m_HumanoidAnimationSoundEvents.OnPlayDualAttack += OnPlayDualAttackServerRpc;
                m_HumanoidAnimationSoundEvents.OnPlayJumpSound += OnPlayJumpSoundServerRpc;
                m_HumanoidAnimationSoundEvents.OnPlayChargeSwordsSound += OnPlayChargeSwordsSoundServerRpc;
                m_HumanoidAnimationSoundEvents.OnPlayDamageGrunt += OnPlayDamageGruntServerRpc;
                m_HumanoidAnimationSoundEvents.OnPlayDead += OnPlayDeadServerRpc;
                
                m_HumanoidAnimationSoundEvents.Init();
            }
            else
            {
                
            }
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void OnRightStepEventServerRpc()
        {
            OnRightStepEventClientRpc();
        }
        
        [ClientRpc]
        private void OnRightStepEventClientRpc()
        {
            if (IsOwner) return;
            m_HumanoidAnimationSoundEvents.OnRightStepAction();
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void OnLeftStepEventServerRpc()
        {
            OnLeftStepEventClientRpc();
        }
        
        [ClientRpc]
        private void OnLeftStepEventClientRpc()
        {
            if (IsOwner) return;
            m_HumanoidAnimationSoundEvents.OnLeftStepAction();
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void OnPlaySwordDrawServerRpc()
        {
            OnPlaySwordDrawClientRpc();
        }
        
        [ClientRpc]
        private void OnPlaySwordDrawClientRpc()
        {
            if (IsOwner) return;
            m_HumanoidAnimationSoundEvents.PlaySwordDrawAction();
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void OnPlayDSlashServerRpc(int trackIndex)
        {
            OnPlayDSlashClientRpc(trackIndex);
        }
        
        [ClientRpc]
        private void OnPlayDSlashClientRpc(int trackIndex)
        {
            if (IsOwner) return;
            m_HumanoidAnimationSoundEvents.PlayDSlashAction(trackIndex);
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void OnPlayFireChargeServerRpc(int trackIndex)
        {
            OnPlayFireChargeClientRpc(trackIndex);
        }
        
        [ClientRpc]
        private void OnPlayFireChargeClientRpc(int trackIndex)
        {
            if (IsOwner) return;
            m_HumanoidAnimationSoundEvents.PlayFireChargeAction(trackIndex);
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void OnPlayDualAttackServerRpc(int trackIndex)
        {
            OnPlayDualAttackClientRpc(trackIndex);
        }
        
        [ClientRpc]
        private void OnPlayDualAttackClientRpc(int trackIndex)
        {
            if (IsOwner) return;
            m_HumanoidAnimationSoundEvents.PlayDualAttackAction(trackIndex);
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void OnPlayJumpSoundServerRpc()
        {
            OnPlayJumpSoundClientRpc();
        }
        
        [ClientRpc]
        private void OnPlayJumpSoundClientRpc()
        {
            if (IsOwner) return;
            m_HumanoidAnimationSoundEvents.PlayJumpSoundAction();
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void OnPlayChargeSwordsSoundServerRpc()
        {
            OnPlayChargeSwordsSoundClientRpc();
        }
        
        [ClientRpc]
        private void OnPlayChargeSwordsSoundClientRpc()
        {
            if (IsOwner) return;
            m_HumanoidAnimationSoundEvents.PlayChargeSwordsSoundAction();
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void OnPlayDamageGruntServerRpc()
        {
            OnPlayDamageGruntClientRpc();
        }
        
        [ClientRpc]
        private void OnPlayDamageGruntClientRpc()
        {
            if (IsOwner) return;
            m_HumanoidAnimationSoundEvents.PlayDamageGruntAction();
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void OnPlayDeadServerRpc()
        {
            OnPlayDeadClientRpc();
        }
        
        [ClientRpc]
        private void OnPlayDeadClientRpc()
        {
            if (IsOwner) return;
            m_HumanoidAnimationSoundEvents.PlayDeadAction();
        }
    }
}