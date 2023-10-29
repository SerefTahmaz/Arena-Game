using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    public InteractionHelper InteractionHelper
    {
        get => _interactionHelper;
        set => _interactionHelper = value;
    }
    
    [SerializeField] private float _radius = .3f;
    [SerializeField] private float _castDistance = 2;
    [SerializeField] private LayerMask _mask;

    private InteractionHelper _interactionHelper;
    private GameObject _currentInteractableObject;
    private IInteractableEvent _interactableEvent;
    private TriggerState _triggerState = TriggerState.None;

    private void Start()
    {
        _interactableEvent = GetComponent<IInteractableEvent>();
    }

    // Update is called once per frame
    void Update()
    {
        var sphereCastOrigin = transform.position + _castDistance*transform.forward;
        var direction = -transform.forward;
        RaycastHit raycastHit;
        if (Physics.SphereCast(sphereCastOrigin, _radius, direction, out raycastHit, _castDistance, _mask))
        {
            if (raycastHit.collider.TryGetComponent(out InteractionTrigger interactionTrigger))
            {
                if(interactionTrigger.GetComponentInParent<TriggerController>() == this) return;
                if (_currentInteractableObject != raycastHit.collider.gameObject)
                {
                    _currentInteractableObject = raycastHit.collider.gameObject;
                    _interactableEvent.InvokeOnObjectCanInteractEnter(_interactionHelper, _currentInteractableObject);
                    _triggerState = TriggerState.OnTriggerEnter;
                }
                else
                {
                    _triggerState = TriggerState.OnTriggerStay;
                }
            }
        }
        else
        {
            _triggerState = TriggerState.None;
            if (_currentInteractableObject != null)
            {
                _triggerState = TriggerState.OnTriggerExit;
                _interactableEvent.InvokeOnObjectCanInteractExit(_interactionHelper, _currentInteractableObject);
            }
            _currentInteractableObject = null;
        }
    }
}

public enum TriggerState
{
    OnTriggerEnter,
    OnTriggerStay,
    OnTriggerExit,
    None
}
