using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour, IInputManager
{
    public float HorizontalAxis => _horizontalAxis;
    public float VerticalAxis => _verticalAxis;
    
    private float _horizontalAxis;
    private float _verticalAxis;

    private Action _onClickEvent;
    private Action _onRKeyEvent;
    private Action _onSpaceKeyEvent;
    private Action _onRightClickDownEvent;
    private Action _onRightClickUpEvent;
    private Action _onShiftKeyDownEvent;
    private Action _onShiftKeyUpEvent;
    private Action _onEKeyDownEvent;
    private Action _onNum1Event;
    private Action _onNum2Event;
    private Action _onNum3Event;
    private Action _onNum4Event;
    private Action _onShiftRightClickEvent;
    private Action _onShiftLeftClickEvent;
    private Action _onCtrlLeftClickEvent;
    private Action _onCtrlRightClickEvent;
    private Action _onFKeyDownEvent;

    private void Update()
    {
        _horizontalAxis = Input.GetAxis("Horizontal");
        _verticalAxis = Input.GetAxis("Vertical");
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            _onRKeyEvent?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _onSpaceKeyEvent?.Invoke();
        }
        if (Input.GetMouseButtonDown(1))
        {
            _onRightClickDownEvent?.Invoke();
        }
        if (Input.GetMouseButtonUp(1))
        {
            _onRightClickUpEvent?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _onShiftKeyDownEvent?.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _onShiftKeyUpEvent?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            _onEKeyDownEvent?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _onNum1Event?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _onNum2Event?.Invoke();
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
            _onShiftLeftClickEvent?.Invoke();
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(1))
        {
            _onCtrlRightClickEvent?.Invoke();
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(0))
        {
            _onCtrlLeftClickEvent?.Invoke();
        }
        if (Input.GetMouseButtonDown(0))
        {
            _onClickEvent?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            _onFKeyDownEvent?.Invoke();
        }
    }

    public void AddListenerToOnClickEvent(Action listener)
    {
        _onClickEvent += listener;
    }
    public void RemoveListenerToOnClickEvent(Action listener)
    {
        _onClickEvent -= listener;
    }
    public void AddListenerToOnRKeyEvent(Action listener)
    {
        _onRKeyEvent += listener;
    }
    public void RemoveListenerToOnRKeyEvent(Action listener)
    {
        _onRKeyEvent -= listener;
    }
    public void AddListenerToOnSpaceKeyEvent(Action listener)
    {
        _onSpaceKeyEvent += listener;
    }
    public void RemoveListenerToOnSpaceKeyEvent(Action listener)
    {
        _onSpaceKeyEvent -= listener;
    }
    public void AddListenerToOnRightDownClickEvent(Action listener)
    {
        _onRightClickDownEvent += listener;
    }
    public void RemoveListenerToOnRightDownClickEvent(Action listener)
    {
        _onRightClickDownEvent -= listener;
    }
    public void AddListenerToOnRightUpClickEvent(Action listener)
    {
        _onRightClickUpEvent += listener;
    }
    public void RemoveListenerToOnRightUpClickEvent(Action listener)
    {
        _onRightClickUpEvent -= listener;
    }

    public void AddListenerToOnShiftKeyDownEvent(Action listener)
    {
        _onShiftKeyDownEvent += listener;
    }

    public void RemoveListenerToOnShiftKeyDownEvent(Action listener)
    {
        _onShiftKeyDownEvent -= listener;
    }

    public void AddListenerToOnShiftKeyUpEvent(Action listener)
    {
        _onShiftKeyUpEvent += listener;
    }

    public void RemoveListenerToOnShiftKeyUpEvent(Action listener)
    {
        _onShiftKeyUpEvent -= listener;
    }

    public void AddListenerToOnEKeyDownEvent(Action listener)
    {
        _onEKeyDownEvent += listener;
    }

    public void RemoveListenerToOnEKeyDownEvent(Action listener)
    {
        _onEKeyDownEvent -= listener;
    }
    
    public void AddListenerToOnNum1Event(Action listener)
    {
        _onNum1Event += listener;
    }

    public void RemoveListenerToOnNum1Event(Action listener)
    {
        _onNum1Event -= listener;
    }
    
    public void AddListenerToOnNum2Event(Action listener)
    {
        _onNum2Event += listener;
    }

    public void RemoveListenerToOnNum2Event(Action listener)
    {
        _onNum2Event -= listener;
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
    
    public void AddListenerToOnShiftRightClickEvent(Action listener)
    {
        _onShiftRightClickEvent += listener;
    }

    public void RemoveListenerToOnShiftRightClickEvent(Action listener)
    {
        _onShiftRightClickEvent -= listener;
    }
    public void AddListenerToOnShiftLeftClickEvent(Action listener)
    {
        _onShiftLeftClickEvent += listener;
    }

    public void RemoveListenerToOnShiftLeftClickEvent(Action listener)
    {
        _onShiftLeftClickEvent -= listener;
    }
    
    public void AddListenerToOnCtrlLeftClickEvent(Action listener)
    {
        _onCtrlLeftClickEvent += listener;
    }

    public void RemoveListenerToOnCtrlLeftClickEvent(Action listener)
    {
        _onCtrlLeftClickEvent -= listener;
    }
    
    public void AddListenerToOnCtrlRightClickEvent(Action listener)
    {
        _onCtrlRightClickEvent += listener;
    }

    public void RemoveListenerToOnCtrlRightClickEvent(Action listener)
    {
        _onCtrlRightClickEvent -= listener;
    }
    
    public void AddListenerToOnFKeyDownEvent(Action listener)
    {
        _onFKeyDownEvent += listener;
    }

    public void RemoveListenerToOnFKeyDownEvent(Action listener)
    {
        _onFKeyDownEvent -= listener;
    }
}
