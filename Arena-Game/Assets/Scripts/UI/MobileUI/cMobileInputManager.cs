using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using UnityEngine;
using UnityEngine.Events;

public class cMobileInputManager : cSingleton<cMobileInputManager>
{
    public Action _onRightLightAttackEvent;
    public Action _onUnArmEvent;
    public Action _onJumpEvent;
    public Action _onLeftLightAttackEvent;
    public Action _onWalkSpeedUpEvent;
    public Action _onWalkSpeedNormalEvent;
    public Action _onDrawLeftItem;
    public Action _onDrawRightItem;
    public Action _onTwoHandedAttackEvent;
    public Action _onEnableRightHandBuffEvent;
    public Action _onEnableLeftHandBuffEvent;
    public Action _onFocusEvent;
    public Vector2 _input;
    public Action _onInteractionEvent;
    
    public void OnRightLightAttackButton()
    {
        _onRightLightAttackEvent.Invoke();
        Debug.Log("Attack");
    }
    
    public void OnLeftLightAttackButton()
    {
        _onLeftLightAttackEvent.Invoke();
    }
    
    public void OnJumpButton()
    {
        _onJumpEvent.Invoke();
    }

    public void OnWalkSpeedUp()
    {
        _onWalkSpeedUpEvent.Invoke();
    }
    
    public void OnWalkSpeedNormal()
    {
        _onWalkSpeedNormalEvent.Invoke();
    }
    
    public void OnSwitchLeftSword()
    {
        _onDrawLeftItem.Invoke();
    }
    
    public void OnSwitchRightSword()
    {
        _onDrawRightItem.Invoke();
    }
    
    public void OnTwoHandedAttack()
    {
        _onTwoHandedAttackEvent.Invoke();
    }
    
    public void OnEnableRightHandBuff()
    {
        _onEnableRightHandBuffEvent.Invoke();
    }
    
    public void OnEnableLeftHandBuff()
    {
        _onEnableLeftHandBuffEvent.Invoke();
    }

    public void OnFocusEvent()
    {
        _onFocusEvent.Invoke();
    }
    
    public void OnInteractionEvent()
    {
        _onInteractionEvent.Invoke();
    }

    private void Update()
    {
        _input = cJoystickController.Instance.JoystickValue;
    }
}
