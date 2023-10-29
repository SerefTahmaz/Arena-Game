using System;
using UnityEngine;

public class PlayerEvent : MonoBehaviour, IInteractableEvent
{
    private Action<InteractionHelper, GameObject> _onPlayerCanInteractEnter;

    public void InvokeOnObjectCanInteractEnter(InteractionHelper interactionHelper, GameObject InteractEnterableObject)
    {
        _onPlayerCanInteractEnter.Invoke(interactionHelper,InteractEnterableObject);
    }

    public void AddListenerToOnObjectCanInteractEnter(Action<InteractionHelper, GameObject> listener)
    {
        _onPlayerCanInteractEnter += listener;
    }
    public void RemoveListenerToOnObjectCanInteractEnter(Action<InteractionHelper, GameObject> listener)
    {
        _onPlayerCanInteractEnter -= listener;
    }
    
    private Action<InteractionHelper, GameObject> _onPlayerCanInteractExit;

    public void InvokeOnObjectCanInteractExit(InteractionHelper interactionHelper, GameObject InteractExitableObject)
    {
        _onPlayerCanInteractExit.Invoke(interactionHelper,InteractExitableObject);
    }

    public void AddListenerToOnObjectCanInteractExit(Action<InteractionHelper, GameObject> listener)
    {
        _onPlayerCanInteractExit += listener;
    }
    public void RemoveListenerToOnObjectCanInteractExit(Action<InteractionHelper, GameObject> listener)
    {
        _onPlayerCanInteractExit -= listener;
    }
}
