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
            m_InteractableUI.SetActive(false);
            InputManager.Instance.RemoveListenerToOnInteractionEvent(HandleInteractionEvent);
            InteractionAvailable = false;
            StartInteraction();
        }

        private void HandleOnPlayerExit()
        {
            m_InteractableUI.SetActive(false);
            InputManager.Instance.RemoveListenerToOnInteractionEvent(HandleInteractionEvent);
            InteractionAvailable = false;
        }

        private void HandleOnPlayerEntered()
        {
            m_InteractableUI.SetActive(true);
            InputManager.Instance.AddListenerToOnInteractionEvent(HandleInteractionEvent);
            InteractionAvailable = true;
        }

        protected virtual async UniTask StartInteraction()
        {
            OnDialogStarted?.Invoke();
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
                InputManager.Instance.AddListenerToOnInteractionEvent(HandleInteractionEvent);
                m_InteractableUI.SetActive(true);
                InteractionAvailable = true;
            }
        }
    }
}