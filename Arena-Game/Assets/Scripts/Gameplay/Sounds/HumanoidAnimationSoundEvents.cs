using System;
using UnityEngine;

namespace DefaultNamespace.SoundManager
{
    public class HumanoidAnimationSoundEvents: MonoBehaviour
    {
        [SerializeField] private bool m_InitAtStart=true;
        [SerializeField] private cSoundEffectController m_SoundEffectController;
        
        public Action OnRightStepEvent { get; set; }
        public Action OnLeftStepEvent { get; set; }
        public Action OnPlaySwordDraw { get; set; }
        public Action<int> OnPlayDSlash { get; set; }
        public Action<int> OnPlayFireCharge { get; set; }
        public Action<int> OnPlayDualAttack { get; set; }
        public Action OnPlayJumpSound { get; set; }
        public Action OnPlayChargeSwordsSound { get; set; }
        public Action OnPlayDamageGrunt { get; set; }
        public Action OnPlayDead { get; set; }
        
        public bool IsInitialized { get; set; }
        
        private void Awake()
        { 
            if (m_InitAtStart)
            {
                Init();
            }
        }

        public void Init()
        {
            IsInitialized = true;
        }
        
        public void OnRightStep()
        {
            if(!IsInitialized) return;
            OnRightStepAction();
            OnRightStepEvent?.Invoke();
        }

        public void OnRightStepAction()
        {
            m_SoundEffectController.OnRightStep();
        }

        public void OnLeftStep()
        {
            if(!IsInitialized) return;
            OnLeftStepAction();
            OnLeftStepEvent?.Invoke();
        }

        public void OnLeftStepAction()
        {
            m_SoundEffectController.OnLeftStep();
        }

        public void PlaySwordDraw()
        {
            if(!IsInitialized) return;
            PlaySwordDrawAction();
            OnPlaySwordDraw?.Invoke();
        }

        public void PlaySwordDrawAction()
        {
            m_SoundEffectController.PlaySwordDraw();
        }

        public void PlayDSlash(int trackIndex)
        {
            if(!IsInitialized) return;
            PlayDSlashAction(trackIndex);
            OnPlayDSlash?.Invoke(trackIndex);
        }

        public void PlayDSlashAction(int trackIndex)
        {
            m_SoundEffectController.PlayDSlash(trackIndex);
        }

        public void PlayFireCharge(int trackIndex)
        {
            if(!IsInitialized) return;
            PlayFireChargeAction(trackIndex);
            OnPlayFireCharge?.Invoke(trackIndex);
        }

        public void PlayFireChargeAction(int trackIndex)
        {
            m_SoundEffectController.PlayFireCharge(trackIndex);
        }

        public void PlayDualAttack(int trackIndex)
        {
            if(!IsInitialized) return;
            PlayDualAttackAction(trackIndex);
            OnPlayDualAttack?.Invoke(trackIndex);
        }

        public void PlayDualAttackAction(int trackIndex)
        {
            m_SoundEffectController.PlayDualAttack(trackIndex);
        }

        public void PlayJumpSound()
        {
            if(!IsInitialized) return;
            PlayJumpSoundAction();
            OnPlayJumpSound?.Invoke();
        }

        public void PlayJumpSoundAction()
        {
            m_SoundEffectController.PlayJumpSound();
        }

        public void PlayChargeSwordsSound()
        {
            if(!IsInitialized) return;
            PlayChargeSwordsSoundAction();
            OnPlayChargeSwordsSound?.Invoke();
        }

        public void PlayChargeSwordsSoundAction()
        {
            m_SoundEffectController.PlayChargeSwordsSound();
        }

        public void PlayDamageGrunt()
        {
            if(!IsInitialized) return;
            PlayDamageGruntAction();
            OnPlayDamageGrunt?.Invoke();
        }

        public void PlayDamageGruntAction()
        {
            m_SoundEffectController.PlayDamageGrunt();
        }

        public void PlayDead()
        {
            if(!IsInitialized) return;
            PlayDeadAction();
            OnPlayDead?.Invoke();
        }

        public void PlayDeadAction()
        {
            m_SoundEffectController.PlayDead();
        }
    }
}