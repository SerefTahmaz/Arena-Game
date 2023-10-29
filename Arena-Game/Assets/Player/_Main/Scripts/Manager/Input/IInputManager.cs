using System;

public interface IInputManager
{
    float HorizontalAxis { get; }
    float VerticalAxis { get; }
    public void AddListenerToOnClickEvent(Action listener);
    public void RemoveListenerToOnClickEvent(Action listener);
    public void AddListenerToOnRKeyEvent(Action listener);
    public void RemoveListenerToOnRKeyEvent(Action listener);
    public void AddListenerToOnSpaceKeyEvent(Action listener);
    public void RemoveListenerToOnSpaceKeyEvent(Action listener);
    public void AddListenerToOnRightDownClickEvent(Action listener);
    public void RemoveListenerToOnRightDownClickEvent(Action listener);
    public void AddListenerToOnRightUpClickEvent(Action listener);
    public void RemoveListenerToOnRightUpClickEvent(Action listener);
    public void AddListenerToOnShiftKeyDownEvent(Action listener);
    public void RemoveListenerToOnShiftKeyDownEvent(Action listener);
    public void AddListenerToOnShiftKeyUpEvent(Action listener);
    public void RemoveListenerToOnShiftKeyUpEvent(Action listener);
    public void AddListenerToOnEKeyDownEvent(Action listener);
    public void RemoveListenerToOnEKeyDownEvent(Action listener);
    
    public void AddListenerToOnNum1Event(Action listener);
    public void RemoveListenerToOnNum1Event(Action listener);
    public void AddListenerToOnNum2Event(Action listener);
    public void RemoveListenerToOnNum2Event(Action listener);
    public void AddListenerToOnNum3Event(Action listener);
    public void RemoveListenerToOnNum3Event(Action listener);
    public void AddListenerToOnNum4Event(Action listener);
    public void RemoveListenerToOnNum4Event(Action listener);
    public void AddListenerToOnShiftRightClickEvent(Action listener);
    public void RemoveListenerToOnShiftRightClickEvent(Action listener);
    public void AddListenerToOnShiftLeftClickEvent(Action listener);
    public void RemoveListenerToOnShiftLeftClickEvent(Action listener);
    public void AddListenerToOnCtrlLeftClickEvent(Action listener);
    public void RemoveListenerToOnCtrlLeftClickEvent(Action listener);
    public void AddListenerToOnCtrlRightClickEvent(Action listener);
    public void RemoveListenerToOnCtrlRightClickEvent(Action listener);
    public void AddListenerToOnFKeyDownEvent(Action listener);
    public void RemoveListenerToOnFKeyDownEvent(Action listener);
}