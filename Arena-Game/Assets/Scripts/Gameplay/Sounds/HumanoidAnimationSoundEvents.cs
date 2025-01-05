using System;
using UnityEngine;

namespace DefaultNamespace.SoundManager
{
    public class HumanoidAnimationSoundEvents: MonoBehaviour
    {
        [SerializeField] private cSoundEffectController m_SoundEffectController;
        
        public void OnRightStep()
        {
            m_SoundEffectController.OnRightStep();
        }
    
        public void OnLeftStep()
        {
            m_SoundEffectController.OnLeftStep();
        }
        
        public void PlaySwordDraw()
        {
            m_SoundEffectController.PlaySwordDraw();
        }
        
        public void PlayDSlash(int trackIndex)
        {
            m_SoundEffectController.PlayDSlash(trackIndex);
        }
        
        public void PlayFireCharge(int trackIndex)
        {
            m_SoundEffectController.PlayFireCharge(trackIndex);
        }
        
        public void PlayDualAttack(int trackIndex)
        {
            m_SoundEffectController.PlayDualAttack(trackIndex);
        }
        
        public void PlayJumpSound()
        {
            m_SoundEffectController.PlayJumpSound();
        }
            
        public void PlayChargeSwordsSound()
        {
            m_SoundEffectController.PlayChargeSwordsSound();
        }
        
        public void PlayDamageGrunt()
        {
            m_SoundEffectController.PlayDamageGrunt();
        }
            
        public void PlayDead()
        {
            m_SoundEffectController.PlayDead();
        }
    }
}