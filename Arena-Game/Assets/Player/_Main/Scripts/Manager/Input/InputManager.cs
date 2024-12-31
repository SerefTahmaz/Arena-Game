using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : cSingleton<InputManager>, IInputManager
{
    [SerializeField] private bool m_MobileInput;
    [SerializeField] private bool m_PCInput;

    private bool m_IsInput;

    public float HorizontalAxis => _horizontalAxis;
    public float VerticalAxis => _verticalAxis;
    
    private float _horizontalAxis;
    private float _verticalAxis;

    private Action _onRightLightAttackEvent;
    private Action _onUnArmEvent;
    private Action _onJumpEvent;
    private Action _onLeftLightAttackEvent;
    private Action _onRightClickUpEvent;
    private Action _onWalkSpeedUpEvent;
    private Action _onWalkSpeedNormalEvent;
    private Action _onDrawRightItem;
    private Action _onDrawLeftItem;
    private Action _onNum3Event;
    private Action _onNum4Event;
    private Action _onShiftRightClickEvent;
    private Action _onTwoHandedAttackEvent;
    private Action _onEnableRightHandBuffEvent;
    private Action _onEnableLeftHandBuffEvent;
    private Action _onFocusCharEvent;
    public Action _onInteractionEvent;

    private void Start()
    {
        if (m_MobileInput)
        {
            cMobileInputManager.Instance._onLeftLightAttackEvent += () =>
            {
                _onLeftLightAttackEvent?.Invoke();
            };
            cMobileInputManager.Instance._onRightLightAttackEvent += () =>
            {
                _onRightLightAttackEvent?.Invoke();
            };
            cMobileInputManager.Instance._onJumpEvent += () =>
            {
                _onJumpEvent?.Invoke();
            };
            cMobileInputManager.Instance._onDrawLeftItem += () =>
            {
                _onDrawLeftItem?.Invoke();
            };
            cMobileInputManager.Instance._onDrawRightItem += () =>
            {
                _onDrawRightItem?.Invoke();
            };
            cMobileInputManager.Instance._onEnableRightHandBuffEvent += () =>
            {
                _onEnableRightHandBuffEvent?.Invoke();
            };
            cMobileInputManager.Instance._onEnableLeftHandBuffEvent += () =>
            {
                _onEnableLeftHandBuffEvent?.Invoke();
            };
            cMobileInputManager.Instance._onWalkSpeedUpEvent += () =>
            {
                _onWalkSpeedUpEvent?.Invoke();
            };
            cMobileInputManager.Instance._onWalkSpeedNormalEvent += () =>
            {
                _onWalkSpeedNormalEvent?.Invoke();
            };
            cMobileInputManager.Instance._onTwoHandedAttackEvent += () =>
            {
                _onTwoHandedAttackEvent?.Invoke();
            };
            cMobileInputManager.Instance._onInteractionEvent += () =>
            {
                _onInteractionEvent?.Invoke();
            };
            cMobileInputManager.Instance._onFocusEvent += () =>
            {
                _onFocusCharEvent?.Invoke();
            };
        }
    }

    public void SetInput(bool value)
    {
        m_IsInput = value;
    }

    private void Update()
    {
        _horizontalAxis = 0;
        _verticalAxis = 0;
        
        if(!m_IsInput) return;
        
        if (m_MobileInput)
        {
            _horizontalAxis = cMobileInputManager.Instance._input.x;
            _verticalAxis = cMobileInputManager.Instance._input.y;
        }
       

        if (m_PCInput)
        {
            _horizontalAxis += Input.GetAxis("Horizontal");
            _verticalAxis += Input.GetAxis("Vertical");
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                _onUnArmEvent?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _onJumpEvent?.Invoke();
            }
            if (Input.GetMouseButtonDown(1))
            {
                _onLeftLightAttackEvent?.Invoke();
            }
            if (Input.GetMouseButtonUp(1))
            {
                _onRightClickUpEvent?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _onWalkSpeedUpEvent?.Invoke();
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                _onWalkSpeedNormalEvent?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                _onInteractionEvent?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _onDrawRightItem?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _onDrawLeftItem?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _onNum3Event?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                _onNum4Event?.Invoke();
            }
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(1))
            {
                _onShiftRightClickEvent?.Invoke();
            }
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
            {
                _onTwoHandedAttackEvent?.Invoke();
            }
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(1))
            {
                _onEnableLeftHandBuffEvent?.Invoke();
            }
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(0))
            {
                _onEnableRightHandBuffEvent?.Invoke();
            }
            if (Input.GetMouseButtonDown(0))
            {
                _onRightLightAttackEvent?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                _onFocusCharEvent?.Invoke();
            }
        }
        
        _horizontalAxis = Mathf.Clamp(_horizontalAxis,-1,1);
        _verticalAxis = Mathf.Clamp(_verticalAxis,-1,1);
    }

    public void AddListenerToOnRightLightAttackEvent(Action listener)
    {
        _onRightLightAttackEvent += listener;
    }
    public void RemoveListenerToOnRightLightAttackEvent(Action listener)
    {
        _onRightLightAttackEvent -= listener;
    }
    public void AddListenerToOnUnArmEvent(Action listener)
    {
        _onUnArmEvent += listener;
    }
    public void RemoveListenerToUnArmEvent(Action listener)
    {
        _onUnArmEvent -= listener;
    }
    public void AddListenerToOnJumpEvent(Action listener)
    {
        _onJumpEvent += listener;
    }
    public void RemoveListenerToOnJumpEvent(Action listener)
    {
        _onJumpEvent -= listener;
    }
    public void AddListenerToOnLeftLightAttackEvent(Action listener)
    {
        _onLeftLightAttackEvent += listener;
    }
    public void RemoveListenerToOnLeftLightAttackEvent(Action listener)
    {
        _onLeftLightAttackEvent -= listener;
    }

    public void AddListenerToOnWalkSpeedUpEvent(Action listener)
    {
        _onWalkSpeedUpEvent += listener;
    }

    public void RemoveListenerToOnWalkSpeedUpEvent(Action listener)
    {
        _onWalkSpeedUpEvent -= listener;
    }

    public void AddListenerToOnWalkSpeedNormalEvent(Action listener)
    {
        _onWalkSpeedNormalEvent += listener;
    }

    public void RemoveListenerToOnWalkSpeedNormalEvent(Action listener)
    {
        _onWalkSpeedNormalEvent -= listener;
    }
    
    public void AddListenerToOnDrawRightItem(Action listener)
    {
        _onDrawRightItem += listener;
    }

    public void RemoveListenerToOnDrawRightItemEvent(Action listener)
    {
        _onDrawRightItem -= listener;
    }
    
    public void AddListenerToOnDrawLeftItem(Action listener)
    {
        _onDrawLeftItem += listener;
    }

    public void RemoveListenerToOnDrawLeftItem(Action listener)
    {
        _onDrawLeftItem -= listener;
    }
    
    public void AddListenerToOnNum3Event(Action listener)
    {
        _onNum3Event += listener;
    }

    public void RemoveListenerToOnNum3Event(Action listener)
    {
        _onNum3Event -= listener;
    }
    
    public void AddListenerToOnNum4Event(Action listener)
    {
        _onNum4Event += listener;
    }

    public void RemoveListenerToOnNum4Event(Action listener)
    {
        _onNum4Event -= listener;
    }
    public void AddListenerToOnTwoHandedAttackEvent(Action listener)
    {
        _onTwoHandedAttackEvent += listener;
    }

    public void RemoveListenerToOnTwoHandedAttackEvent(Action listener)
    {
        _onTwoHandedAttackEvent -= listener;
    }
    
    public void AddListenerToOnEnableRightHandBuffEvent(Action listener)
    {
        _onEnableRightHandBuffEvent += listener;
    }

    public void RemoveListenerToOnEnableRightHandBuffEvent(Action listener)
    {
        _onEnableRightHandBuffEvent -= listener;
    }
    
    public void AddListenerToOnEnableLeftHandBuffEvent(Action listener)
    {
        _onEnableLeftHandBuffEvent += listener;
    }

    public void RemoveListenerToOnEnableLeftHandBuffEvent(Action listener)
    {
        _onEnableLeftHandBuffEvent -= listener;
    }
    
    public void AddListenerToOnFocusCharEvent(Action listener)
    {
        _onFocusCharEvent += listener;
    }

    public void RemoveListenerToOnFocusCharEvent(Action listener)
    {
        _onFocusCharEvent -= listener;
    }
    
    public void AddListenerToOnInteractionEvent(Action listener)
    {
        _onInteractionEvent += listener;
    }

    public void RemoveListenerToOnInteractionEvent(Action listener)
    {
        _onInteractionEvent -= listener;
    }
}
