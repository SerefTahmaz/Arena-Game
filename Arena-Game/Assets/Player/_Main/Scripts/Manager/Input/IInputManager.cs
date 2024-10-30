using System;

public interface IInputManager
{
    float HorizontalAxis { get; }
    float VerticalAxis { get; }
    public void AddListenerToOnRightLightAttackEvent(Action listener);
    public void RemoveListenerToOnRightLightAttackEvent(Action listener);
    public void AddListenerToOnLeftLightAttackEvent(Action listener);
    public void RemoveListenerToOnLeftLightAttackEvent(Action listener);
    public void AddListenerToOnUnArmEvent(Action listener);
    public void RemoveListenerToUnArmEvent(Action listener);
    public void AddListenerToOnJumpEvent(Action listener);
    public void RemoveListenerToOnJumpEvent(Action listener);
    public void AddListenerToOnWalkSpeedUpEvent(Action listener);
    public void RemoveListenerToOnWalkSpeedUpEvent(Action listener);
    public void AddListenerToOnWalkSpeedNormalEvent(Action listener);
    public void RemoveListenerToOnWalkSpeedNormalEvent(Action listener);
    
    public void AddListenerToOnDrawRightItem(Action listener);
    public void RemoveListenerToOnDrawRightItemEvent(Action listener);
    public void AddListenerToOnDrawLeftItem(Action listener);
    public void RemoveListenerToOnDrawLeftItem(Action listener);
    public void AddListenerToOnNum3Event(Action listener);
    public void RemoveListenerToOnNum3Event(Action listener);
    public void AddListenerToOnNum4Event(Action listener);
    public void RemoveListenerToOnNum4Event(Action listener);
    public void AddListenerToOnTwoHandedAttackEvent(Action listener);
    public void RemoveListenerToOnTwoHandedAttackEvent(Action listener);
    public void AddListenerToOnEnableRightHandBuffEvent(Action listener);
    public void RemoveListenerToOnEnableRightHandBuffEvent(Action listener);
    public void AddListenerToOnEnableLeftHandBuffEvent(Action listener);
    public void RemoveListenerToOnEnableLeftHandBuffEvent(Action listener);
    public void AddListenerToOnFKeyDownEvent(Action listener);
    public void RemoveListenerToOnFKeyDownEvent(Action listener);
    public void AddListenerToOnInteractionEvent(Action listener);
    public void RemoveListenerToOnInteractionEvent(Action listener);
}