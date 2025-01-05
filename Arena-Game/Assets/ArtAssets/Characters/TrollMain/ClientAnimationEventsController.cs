using Unity.Netcode;
using UnityEngine;

namespace ArtAssets.Characters.TrollMain
{
    public class ClientAnimationEventsController : NetworkBehaviour
    {
        [SerializeField] private cAnimationEventsController m_AnimationEventsController;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsHost)
            {
                m_AnimationEventsController.OnLeftStepEvent += OnLeftStepEventClientRpc;
                m_AnimationEventsController.OnRightStepEvent += OnRightStepEventClientRpc;
                m_AnimationEventsController.OnGroundAttackEvent += OnGroundAttackEventClientRpc;
                m_AnimationEventsController.OnInitSwordTrail += OnInitSwordTrailClientRpc;
                m_AnimationEventsController.OnInitHammerCrack += OnInitHammerCrackClientRpc;
                m_AnimationEventsController.OnAttack1StartEvent += OnAttack1StartEventClientRpc;
                m_AnimationEventsController.On360AttackStartEvent += On360AttackStartEventClientRpc;
                m_AnimationEventsController.OnHeavyAttack1StartEvent += OnHeavyAttack1StartEventClientRpc;
                m_AnimationEventsController.OnJumpAttackStartEvent += OnJumpAttackStartEventClientRpc;
                m_AnimationEventsController.OnRoarFrontFaceEvent += OnRoarFrontFaceEventClientRpc;
                m_AnimationEventsController.OnRoarEvent += OnRoarEventClientRpc;
                m_AnimationEventsController.OnStartDamage += OnStartDamageClientRpc;
                
                m_AnimationEventsController.Init();
            }
            else
            {
                
            }
        }
        
        [ClientRpc]
        private void OnLeftStepEventClientRpc()
        {
            if (IsHost) return;
            m_AnimationEventsController.OnLeftStepAction();
        }
        
        [ClientRpc]
        private void OnRightStepEventClientRpc()
        {
            if (IsHost) return;
            m_AnimationEventsController.OnRightStepAction();
        }
        
        [ClientRpc]
        private void OnGroundAttackEventClientRpc()
        {
            if (IsHost) return;
            m_AnimationEventsController.OnGroundAttackAction();
        }
        
        [ClientRpc]
        private void OnInitSwordTrailClientRpc(string direction)
        {
            if (IsHost) return;
            m_AnimationEventsController.InitSwordTrailAction(direction);
        }
        
        [ClientRpc]
        private void OnInitHammerCrackClientRpc()
        {
            if (IsHost) return;
            m_AnimationEventsController.InitHammerCrackAction();
        }
        
        [ClientRpc]
        private void OnAttack1StartEventClientRpc()
        {
            if (IsHost) return;
            m_AnimationEventsController.OnAttack1StartAction();
        }
        
        [ClientRpc]
        private void On360AttackStartEventClientRpc()
        {
            if (IsHost) return;
            m_AnimationEventsController.On360AttackStartAction();
        }
        
        [ClientRpc]
        private void OnHeavyAttack1StartEventClientRpc()
        {
            if (IsHost) return;
            m_AnimationEventsController.OnHeavyAttack1StartAction();
        }
        
        [ClientRpc]
        private void OnJumpAttackStartEventClientRpc()
        {
            if (IsHost) return;
            m_AnimationEventsController.OnJumpAttackStartAction();
        }
        
        [ClientRpc]
        private void OnRoarFrontFaceEventClientRpc()
        {
            if (IsHost) return;
            m_AnimationEventsController.OnRoarFrontFaceAction();
        }
        
        [ClientRpc]
        private void OnRoarEventClientRpc()
        {
            if (IsHost) return;
            m_AnimationEventsController.OnRoarAction();
        }
        
        [ClientRpc]
        private void OnStartDamageClientRpc()
        {
            if (IsHost) return;
            m_AnimationEventsController.StartDamageAction();
        }
    }
}