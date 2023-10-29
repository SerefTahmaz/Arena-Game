using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private PlayerEvent _playerEvent;

    private void Awake()
    {
        var events = FindObjectsOfType<MonoBehaviour>()
            .Where((behaviour => behaviour.GetComponent<IInteractableEvent>() != null)).ToList();
        Debug.Log(events.Count);
        foreach (var VARIABLE in events)
        {
            VARIABLE.GetComponent<IInteractableEvent>().AddListenerToOnObjectCanInteractEnter(OnPlayerHit);
            VARIABLE.GetComponent<IInteractableEvent>().AddListenerToOnObjectCanInteractExit(OnPlayerHitExit);
        }
    }
    public void OnPlayerHit(InteractionHelper interactionHelper, GameObject o)
    {
        var interactable = o.GetComponentInParent<IInteractable>();
        if (interactable != null)
        {
            interactable.OnPreviewInteract(interactionHelper);
        }
    }
    public void OnPlayerHitExit(InteractionHelper interactionHelper, GameObject o)
    {
        var interactable = o.GetComponentInParent<IInteractable>();
        if (interactable != null)
        {
            interactable.OnExitInteract(interactionHelper);
        }
    }
}

public enum Actor
{
    Blue,
    Red,
    Player
}
