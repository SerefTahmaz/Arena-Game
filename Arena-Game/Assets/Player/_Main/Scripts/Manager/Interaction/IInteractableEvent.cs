using System;
using UnityEngine;

public interface IInteractableEvent
{
    void InvokeOnObjectCanInteractEnter(InteractionHelper InteractionHelper, GameObject InteractEnterableObject);
    void AddListenerToOnObjectCanInteractEnter(Action<InteractionHelper, GameObject> listener);
    void RemoveListenerToOnObjectCanInteractEnter(Action<InteractionHelper, GameObject> listener);
    void InvokeOnObjectCanInteractExit(InteractionHelper InteractionHelper, GameObject InteractEnterableObject);
    void AddListenerToOnObjectCanInteractExit(Action<InteractionHelper, GameObject> listener);
    void RemoveListenerToOnObjectCanInteractExit(Action<InteractionHelper, GameObject> listener);
}