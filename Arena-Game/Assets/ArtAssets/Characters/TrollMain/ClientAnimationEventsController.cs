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
            m_AnimationEventsController.OnLeftStepAction();
        }
        
        [ClientRpc]
        private void OnRightStepEventClientRpc()
        {
            m_AnimationEventsController.OnRightStepAction();
        }
        
        [ClientRpc]
        private void OnGroundAttackEventClientRpc()
        {
            m_AnimationEventsController.OnGroundAttackAction();
        }
        
        [ClientRpc]
        private void OnInitSwordTrailClientRpc(string direction)
        {
            m_AnimationEventsController.InitSwordTrailAction(direction);
        }
        
        [ClientRpc]
        private void OnInitHammerCrackClientRpc()
        {
            m_AnimationEventsController.InitHammerCrackAction();
        }
        
        [ClientRpc]
        private void OnAttack1StartEventClientRpc()
        {
            m_AnimationEventsController.OnAttack1StartAction();
        }
        
        [ClientRpc]
        private void On360AttackStartEventClientRpc()
        {
            m_AnimationEventsController.On360AttackStartAction();
        }
        
        [ClientRpc]
        private void OnHeavyAttack1StartEventClientRpc()
        {
            m_AnimationEventsController.OnHeavyAttack1StartAction();
        }
        
        [ClientRpc]
        private void OnJumpAttackStartEventClientRpc()
        {
            m_AnimationEventsController.OnJumpAttackStartAction();
        }
        
        [ClientRpc]
        private void OnRoarFrontFaceEventClientRpc()
        {
            m_AnimationEventsController.OnRoarFrontFaceAction();
        }
        
        [ClientRpc]
        private void OnRoarEventClientRpc()
        {
            m_AnimationEventsController.OnRoarAction();
        }
        
        [ClientRpc]
        private void OnStartDamageClientRpc()
        {
            m_AnimationEventsController.StartDamageAction();
        }
    }
}