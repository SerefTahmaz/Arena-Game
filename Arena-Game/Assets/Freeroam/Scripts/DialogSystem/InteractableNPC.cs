using System;
using Cysharp.Threading.Tasks;
using Dialogue;
using UnityEngine;

namespace DefaultNamespace
{
    public class InteractableNPC : MonoBehaviour
    {
        [SerializeField] protected PlayerDetector m_PlayerDetector;
        [SerializeField] protected DialogueGraph m_DialogueGraph;
        [SerializeField] private Transform m_DialogFocusPoint;
        [SerializeField] private GameObject m_InteractableUI;

        protected DialogController m_DialogController;
        
        public bool InteractionAvailable { get; set; }
        public Action OnDialogStarted { get; set; }
        public Action OnDialogEnded { get; set; }
        
        protected virtual void Start()
        {
            m_PlayerDetector.OnPlayerEntered += HandleOnPlayerEntered;
            m_PlayerDetector.OnPlayerExit += HandleOnPlayerExit;
        }

        private void HandleInteractionEvent()
        {
            if(FindObjectOfType<DialogController>()) return;
            
            PlayerInteractionHelper.Instance.RemoveInteractionList(this);
            StartInteraction();
        }

        private void HandleOnPlayerExit()
        {
            PlayerInteractionHelper.Instance.RemoveInteractionList(this);
        }

        private void HandleOnPlayerEntered()
        {
            PlayerInteractionHelper.Instance.AddInteractionList(this);
        }

        protected virtual async UniTask StartInteraction()
        {
            OnDialogStarted?.Invoke();
            PlayerInteractionHelper.Instance.HandleDialogStarted(this);
            m_DialogController = DialogController.CreateInstanceDialog();
            await m_DialogController.Init(m_DialogueGraph, m_DialogFocusPoint);
            await UniTask.WaitForSeconds(0.1f);
            HandleOnDialogEnded();
        }

        protected virtual void HandleOnDialogEnded()
        {
            if (m_PlayerDetector.IsPlayerInside)
            {
                OnDialogEnded?.Invoke();
                PlayerInteractionHelper.Instance.AddInteractionList(this);
            }
            PlayerInteractionHelper.Instance.HandleDialogEnded(this);
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
}