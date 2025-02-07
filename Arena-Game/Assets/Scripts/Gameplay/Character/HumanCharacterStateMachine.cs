using System;
using System.Collections;
using System.Collections.Generic;
using _Main.Scripts.Gameplay;
using Gameplay.Character;
using UnityEngine;
using Random = UnityEngine.Random;

public class HumanCharacterStateMachine : MonoBehaviour
{
    [SerializeField] private HumanCharacter m_HumanCharacter;
    [SerializeField] private bool m_InitAtStart;

    private AnimationController AnimationController => m_HumanCharacter.AnimationController;

    public bool IsLeftSwordDrawn { get; set; }

    public bool IsRightSwordDrawn { get; set; }

    public bool IsLeftSwordCharged { get; set; }

    public bool IsRightSwordCharged { get; set; }

    private bool IsBusy;
    
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
        AnimationController.m_OnAttackEnd += () =>
        {
            IsBusy = false;
        };
        IsInitialized = true;
    }

    //Abilities
    
    public void SwitchLeftSword()
    {
        if(!IsInitialized) return;
        
        if(IsBusy) return;
        
        if (IsLeftSwordDrawn)
        {
            SheathSword(AnimationController.AnimationState.SheathLeftSword);
        }
        else
        {
            DrawSword(AnimationController.AnimationState.DrawLeftSword);
        }
    }
        
    public void SwitchRightSword()
    {
        if(!IsInitialized) return;
        
        if(IsBusy) return;
        
        if (IsRightSwordDrawn)
        {
            SheathSword(AnimationController.AnimationState.SheathRightSword);
        }
        else
        {
            DrawSword(AnimationController.AnimationState.DrawRightSword);
        }
    }
        
    private void DrawSword(AnimationController.AnimationState sword)
    {
        if(!IsInitialized) return;
        
        AnimationController.SetTrigger(sword, resetable: true);
    }
        
    private void SheathSword(AnimationController.AnimationState sword)
    {
        if(!IsInitialized) return;
        
        AnimationController.SetTrigger(sword, resetable: true);
    }

    void LeftSlash()
    {
        if(!IsInitialized) return;
        
        SetBusy();
        AnimationController.SetTrigger(AnimationController.AnimationState.LeftSlash, resetable: true);
    }
        
    void RightSlash()
    {
        if(!IsInitialized) return;
        
        SetBusy();
        AnimationController.SetTrigger(AnimationController.AnimationState.Slash, resetable: true);
    }
        
    public void Slash()
    {
        if(!IsInitialized) return;
        
        if (IsLeftSwordDrawn && IsRightSwordDrawn)
        {
            HeavyAttack();
        }
        else if (IsRightSwordDrawn)
        {
            RightSlash();
        }
        else if (IsLeftSwordDrawn)
        {
            LeftSlash();
        }
    }
        
    private void HeavyAttack()
    {
        if(!IsInitialized) return;
        
        SetBusy();
        AnimationController.SetTrigger(AnimationController.AnimationState.HeavySlash, resetable: true);
    }
        
    private void OnChargeLeft()
    {
        if(!IsInitialized) return;
        
        SetBusy();
        AnimationController.SetTrigger(AnimationController.AnimationState.ChargeLeft, resetable: true);
    }
        
    private void OnChargeRight()
    {
        if(!IsInitialized) return;
        
        SetBusy();
        AnimationController.SetTrigger(AnimationController.AnimationState.ChargeRight, resetable: true);
    }
        
    public void Charge()
    {
        if(!IsInitialized) return;
        
        Debug.Log("Trying to charge!!");
        if (IsLeftSwordDrawn && IsRightSwordDrawn)
        {
            OnChargeBoth();
        }
        else if (IsRightSwordDrawn)
        {
            OnChargeRight();
        }
        else if (IsLeftSwordDrawn)
        {
            OnChargeLeft();
        }
    }
        
    private void OnChargeBoth()
    {
        if(!IsInitialized) return;
        
        SetBusy();
        AnimationController.SetTrigger(AnimationController.AnimationState.ChargeBoth, resetable: true);
    }

    public void Jump()
    {
        if(!IsInitialized) return;
        
        AnimationController.SetTrigger(AnimationController.AnimationState.Jump, resetable: true);
    }

    public void PlayTakeDamage(bool isHeavyDamage)
    {
        if(!IsInitialized) return;
        
        SetBusy();
        AnimationController.SetTrigger(isHeavyDamage ? AnimationController.AnimationState.BackImpact : AnimationController.AnimationState.Damage, 
            resetable: true);
        AnimationController.SetTrigger(AnimationController.AnimationState.DamageAnimIndex, Random.Range(0, 2));
    }

    private void SetBusy()
    {
        if(!IsInitialized) return;
        
        IsBusy = true;
    }
    
    //Animation Events
    
    public Action OnSwitchLeftWeapon { get; set; }
    public Action OnSwitchRightWeapon { get; set; }
    
    public void SwitchLeftWeapon()
    {
        if(!IsInitialized) return;
        
        IsLeftSwordDrawn = !IsLeftSwordDrawn;
        SetLeftWeaponVisuals(IsLeftSwordDrawn);
        SetLeftFlame(0);
        
        OnSwitchLeftWeapon?.Invoke();
    }
    
    public void SwitchRightWeapon()
    {
        if(!IsInitialized) return;
        
        IsRightSwordDrawn = !IsRightSwordDrawn;
        SetRightWeaponVisuals(IsRightSwordDrawn);
        SetRightFlame(0);
        
        OnSwitchRightWeapon?.Invoke();
    }

    public void SetLeftWeaponVisuals(bool value)
    {
        AnimationController.LeftSword.m_BackWeapon.SetActive(!value);
        AnimationController.LeftSword.m_HandWeapon.SetActive(value);
    }

    public void SetRightWeaponVisuals(bool value)
    {
        AnimationController.RightSword.m_BackWeapon.SetActive(!value);
        AnimationController.RightSword.m_HandWeapon.SetActive(value);
    }
    
    public Action OnSetLeftFlame { get; set; }
    public Action OnSetRightFlame { get; set; }
    
    public void SetLeftFlame(int state)
    {
        if(!IsInitialized) return;
        
        IsLeftSwordCharged = state == 1 ? true : false;
        SetLeftFlameVisuals(IsLeftSwordCharged);
        
        OnSetLeftFlame?.Invoke();
    }
    
    
    public void SetRightFlame(int state)
    {
        if(!IsInitialized) return;
        
        IsRightSwordCharged = state == 1 ? true : false;
        SetRightFlameVisuals(IsRightSwordCharged);
        
        OnSetRightFlame?.Invoke();
    }
    
    public void SetLeftFlameVisuals(bool value)
    {
        SetFlame(AnimationController.LeftSword, value);
    }

    public void SetRightFlameVisuals(bool value)
    {
        SetFlame(AnimationController.RightSword, value);
    } 
    
    public void SetFlame(AnimationController.SwordFlameHelper swordFlameHelper, bool state)
    {
        if (state)
        {
            swordFlameHelper.m_FlameParticle.gameObject.SetActive(true);
            swordFlameHelper.m_FlameParticle.Play();
            swordFlameHelper.handWeaponRenderer.sharedMaterial = swordFlameHelper.m_BurntMat;
            swordFlameHelper.SetFlameLoopSound(true);
        }
        else
        {
            swordFlameHelper.m_FlameParticle.gameObject.SetActive(false);
            swordFlameHelper.m_FlameParticle.Stop();
            swordFlameHelper.handWeaponRenderer.sharedMaterial = swordFlameHelper.m_NormalMat;
            swordFlameHelper.SetFlameLoopSound(false);
        }
    }

    public void PlayStreach()
    {
        if(!IsInitialized) return;
    }

    public void PlayHelloEverybody()
    {
        if(!IsInitialized) return;
    }

    public void Die()
    {
        if(!IsInitialized) return;
        
        AnimationController.SetTrigger(AnimationController.AnimationState.Dead);
    }
}