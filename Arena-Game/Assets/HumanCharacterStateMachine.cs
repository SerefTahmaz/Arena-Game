using System;
using System.Collections;
using System.Collections.Generic;
using _Main.Scripts.Gameplay;
using Gameplay.Character;
using UnityEngine;

public class HumanCharacterStateMachine : MonoBehaviour
{
    [SerializeField] private HumanCharacter m_HumanCharacter;

    private AnimationController AnimationController => m_HumanCharacter.AnimationController;
    
    private bool m_IsLeftSwordDrawn = false;
    private bool m_IsRightSwordDrawn = false;

    private bool IsBusy;
    
    private void Awake()
    {
        AnimationController.m_OnAttackEnd += () =>
        {
            IsBusy = false;
        };
    }

    //Abilities
    
    public void SwitchLeftSword()
    {
        if(IsBusy) return;
        
        if (m_IsLeftSwordDrawn)
        {
            SheathSword(AnimationController.AnimationState.SheathLeftSword);
        }
        else
        {
            DrawSword(AnimationController.AnimationState.DrawLeftSword);
        }

        m_IsLeftSwordDrawn = !m_IsLeftSwordDrawn;
    }
        
    public void SwitchRightSword()
    {
        if(IsBusy) return;
        
        if (m_IsRightSwordDrawn)
        {
            SheathSword(AnimationController.AnimationState.SheathRightSword);
        }
        else
        {
            DrawSword(AnimationController.AnimationState.DrawRightSword);
        }

        m_IsRightSwordDrawn = !m_IsRightSwordDrawn;
    }
        
    public void DrawSword(AnimationController.AnimationState sword)
    {
        AnimationController.SetTrigger(sword);
    }
        
    public void SheathSword(AnimationController.AnimationState sword)
    {
        AnimationController.SetTrigger(sword);
    }

    void LeftSlash()
    {
        SetBusy();
        AnimationController.SetTrigger(AnimationController.AnimationState.LeftSlash, resetable: true);
    }
        
    void RightSlash()
    {
        SetBusy();
        AnimationController.SetTrigger(AnimationController.AnimationState.Slash, resetable: true);
    }
        
    public void Slash()
    {
        if (m_IsLeftSwordDrawn && m_IsRightSwordDrawn)
        {
            HeavyAttack();
        }
        else if (m_IsRightSwordDrawn)
        {
            RightSlash();
        }
        else if (m_IsLeftSwordDrawn)
        {
            LeftSlash();
        }
    }
        
    private void HeavyAttack()
    {
        SetBusy();
        AnimationController.SetTrigger(AnimationController.AnimationState.HeavySlash, resetable: true);
    }
        
    private void OnChargeLeft()
    {
        SetBusy();
        AnimationController.SetTrigger(AnimationController.AnimationState.ChargeLeft, resetable: true);
    }
        
    private void OnChargeRight()
    {
        SetBusy();
        AnimationController.SetTrigger(AnimationController.AnimationState.ChargeRight, resetable: true);
    }
        
    public void Charge()
    {
        if (m_IsLeftSwordDrawn && m_IsRightSwordDrawn)
        {
            OnChargeBoth();
        }
        else if (m_IsRightSwordDrawn)
        {
            OnChargeRight();
        }
        else if (m_IsLeftSwordDrawn)
        {
            OnChargeLeft();
        }
    }
        
    private void OnChargeBoth()
    {
        SetBusy();
        AnimationController.SetTrigger(AnimationController.AnimationState.ChargeBoth, resetable: true);
    }

    public void Jump()
    {
        AnimationController.SetTrigger(AnimationController.AnimationState.Jump, resetable: true);
    }

    private void SetBusy()
    {
        IsBusy = true;
    }
    
    //Animation Events
    
    public void SwitchLeftWeapon()
    {
        AnimationController.LeftSword.m_BackWeapon.SetActive(!AnimationController.LeftSword.m_BackWeapon.activeSelf);
        AnimationController.LeftSword.m_HandWeapon.SetActive(!AnimationController.LeftSword.m_HandWeapon.activeSelf);
        SetLeftFlame(0);
    }
    
    public void SwitchRightWeapon()
    {
        AnimationController.RightSword.m_BackWeapon.SetActive(!AnimationController.RightSword.m_BackWeapon.activeSelf);
        AnimationController.RightSword.m_HandWeapon.SetActive(!AnimationController.RightSword.m_HandWeapon.activeSelf);
        SetRightFlame(0);
    }
    
    public void SetLeftFlame(int state)
    {
        SetFlame(AnimationController.LeftSword, state == 1 ? true : false);
    }
    
    
    public void SetRightFlame(int state)
    {
        SetFlame(AnimationController.RightSword, state == 1 ? true : false);
    }
    
    private void SetFlame(AnimationController.SwordFlameHelper swordFlameHelper, bool state)
    {
        if (state)
        {
            swordFlameHelper.m_FlameParticle.gameObject.SetActive(true);
            swordFlameHelper.m_FlameParticle.Play();
            swordFlameHelper.handWeaponRenderer.sharedMaterial = swordFlameHelper.m_BurntMat;
        }
        else
        {
            swordFlameHelper.m_FlameParticle.gameObject.SetActive(false);
            swordFlameHelper.m_FlameParticle.Stop();
            swordFlameHelper.handWeaponRenderer.sharedMaterial = swordFlameHelper.m_NormalMat;
        }
    }

    public void PlayStreach()
    {
    }

    public void PlayHelloEverybody()
    {
    }

    public void Die()
    {
        AnimationController.SetTrigger(AnimationController.AnimationState.Dead);
    }
}