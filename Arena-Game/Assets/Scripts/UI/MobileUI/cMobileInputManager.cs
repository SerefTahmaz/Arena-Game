using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class cMobileInputManager : MonoBehaviour
{
    public static Action _onRightLightAttackEvent;
    public static Action _onUnArmEvent;
    public static Action _onJumpEvent;
    public static Action _onLeftLightAttackEvent;
    public static Action _onWalkSpeedUpEvent;
    public static Action _onWalkSpeedNormalEvent;
    public static Action _onDrawLeftItem;
    public static Action _onDrawRightItem;
    public static Action _onTwoHandedAttackEvent;
    public static Action _onEnableRightHandBuffEvent;
    public static Action _onEnableLeftHandBuffEvent;
    public static Action _onFocusEvent;
    public static Vector2 _input;
    public static Action _onInteractionEvent;
    
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
        _input = cJoystickController.JoystickValue;
    }
}
