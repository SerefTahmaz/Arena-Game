using System;
using Unity.Netcode;
using UnityEngine;

namespace ArtAssets.Characters.Creature.Scripts
{
    public class ClientCreatureAnimationEventsController : NetworkBehaviour
    {
        [SerializeField] private cCreatureAnimationEventsController m_CreatureAnimationEventsController;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsHost)
            {
                m_CreatureAnimationEventsController.OnRolImpact += OnRolImpactClientRpc;
                m_CreatureAnimationEventsController.OnRightSlashStart += OnRightSlashStartClientRpc;
                m_CreatureAnimationEventsController.OnRightSlashEnd += OnRightSlashEndClientRpc;
                m_CreatureAnimationEventsController.OnLeftSlashStart += OnLeftSlashStartClientRpc;
                m_CreatureAnimationEventsController.OnBothSlashStart += OnBothSlashStartClientRpc;
                m_CreatureAnimationEventsController.OnBothSlashEnd += OnBothSlashEndClientRpc;
                m_CreatureAnimationEventsController.OnLeftStepEvent += OnLeftStepEventClientRpc;
                m_CreatureAnimationEventsController.OnRightStepEvent += OnRightStepEventClientRpc;
                m_CreatureAnimationEventsController.OnClawSlashStartEvent += OnClawSlashStartEventClientRpc;
                m_CreatureAnimationEventsController.OnMutantSwipingStartEvent += OnMutantSwipingStartEventClientRpc;
                m_CreatureAnimationEventsController.OnSpinAttackStartEvent += OnSpinAttackStartEventClientRpc;
                m_CreatureAnimationEventsController.OnComboAttackStartEvent += OnComboAttackStartEventClientRpc;
                m_CreatureAnimationEventsController.OnRollStartEvent += OnRollStartEventClientRpc;
                m_CreatureAnimationEventsController.OnDeadStart += OnDeadStartClientRpc;
                m_CreatureAnimationEventsController.OnHitBodyStart += OnHitBodyStartClientRpc;
                
                m_CreatureAnimationEventsController.Init();
            }
            else
            {
                
            }
        }
        
        [ClientRpc]
        private void OnRolImpactClientRpc()
        {
            m_CreatureAnimationEventsController.RolImpactAction();
        }
        
        [ClientRpc]
        private void OnRightSlashStartClientRpc()
        {
            m_CreatureAnimationEventsController.RightSlashStartAction();
        }
        
        [ClientRpc]
        private void OnRightSlashEndClientRpc()
        {
            m_CreatureAnimationEventsController.RightSlashEndAction();
        }
        
        [ClientRpc]
        private void OnLeftSlashStartClientRpc()
        {
            m_CreatureAnimationEventsController.LeftSlashStartAction();
        }
        
        [ClientRpc]
        private void OnBothSlashStartClientRpc()
        {
            m_CreatureAnimationEventsController.BothSlashStartAction();
        }
        
        [ClientRpc]
        private void OnBothSlashEndClientRpc()
        {
            m_CreatureAnimationEventsController.BothSlashEndAction();
        }
        
        [ClientRpc]
        private void OnLeftStepEventClientRpc()
        {
            m_CreatureAnimationEventsController.OnLeftStepAction();
        }
        
        [ClientRpc]
        private void OnRightStepEventClientRpc()
        {
            m_CreatureAnimationEventsController.OnRightStepAction();
        }
        
        [ClientRpc]
        private void OnClawSlashStartEventClientRpc()
        {
            m_CreatureAnimationEventsController.OnClawSlashStartAction();
        }
        
        [ClientRpc]
        private void OnMutantSwipingStartEventClientRpc()
        {
            m_CreatureAnimationEventsController.OnMutantSwipingStartAction();
        }
        
        [ClientRpc]
        private void OnSpinAttackStartEventClientRpc()
        {
            m_CreatureAnimationEventsController.OnSpinAttackStartAction();
        }
        
        [ClientRpc]
        private void OnComboAttackStartEventClientRpc()
        {
            m_CreatureAnimationEventsController.OnComboAttackStartAction();
        }
        
        [ClientRpc]
        private void OnRollStartEventClientRpc()
        {
            m_CreatureAnimationEventsController.OnRollStartAction();
        }
        
        [ClientRpc]
        private void OnDeadStartClientRpc()
        {
            m_CreatureAnimationEventsController.DeadStartAction();
        }
        
        [ClientRpc]
        private void OnHitBodyStartClientRpc()
        {
            m_CreatureAnimationEventsController.HitBodyStartAction();
        }
    }
}