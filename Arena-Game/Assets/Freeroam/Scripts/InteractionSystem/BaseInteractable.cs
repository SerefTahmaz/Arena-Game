using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BaseInteractable : MonoBehaviour
{
    [SerializeField] protected PlayerDetector m_PlayerDetector;
    [SerializeField] private GameObject m_InteractableUI;
        
    public bool InteractionAvailable { get; set; }
    public Action OnInteractionStarted { get; set; }
    public Action OnInteractionEnded { get; set; }
    
    public virtual InteractionHelper<PlayerInteractionHelper> InteractionHelper { get; set; }
        
    private void Start()
    {
        m_PlayerDetector.OnPlayerEntered += HandleOnPlayerEntered;
        m_PlayerDetector.OnPlayerExit += HandleOnPlayerExit;
    }

    protected virtual void HandleInteractionEvent()
    {
        InteractionHelper.RemoveInteractionList(this);
        StartInteraction();
    }

    private void HandleOnPlayerExit()
    {
        InteractionHelper.RemoveInteractionList(this);
    }

    private void HandleOnPlayerEntered()
    {
        InteractionHelper.AddInteractionList(this);
    }

    protected virtual async UniTask StartInteraction()
    {
        OnInteractionStarted?.Invoke();
        InteractionHelper.HandleInteractionStarted(this);
        await HandleInteraction();
        await UniTask.WaitForSeconds(0.1f);
        HandleOnDialogEnded();
    }

    protected virtual async UniTask HandleInteraction()
    {
        
    }

    protected virtual void HandleOnDialogEnded()
    {
        if (m_PlayerDetector.IsPlayerInside)
        {
            OnInteractionEnded?.Invoke();
            InteractionHelper.AddInteractionList(this);
        }
        InteractionHelper.HandleInteractionEnded(this);
    }

    public void SetInteraction(bool value)
    {
        if (value)
        {
            m_InteractableUI.SetActive(true);
            InputManager.Instance.AddListenerToOnInteractionEvent(HandleInteractionEvent);
            InteractionAvailable = true;
        }
        else
        {
            m_InteractableUI.SetActive(false);
            InputManager.Instance.RemoveListenerToOnInteractionEvent(HandleInteractionEvent);
            InteractionAvailable = false;
        }
    }
}