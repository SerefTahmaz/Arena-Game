using System;
using Cysharp.Threading.Tasks;
using Dialogue;
using UnityEngine;

namespace DefaultNamespace
{
    public class InteractableNPC : BaseInteractable
    {
        [SerializeField] protected DialogueGraph m_DialogueGraph;
        [SerializeField] private Transform m_DialogFocusPoint;

        protected DialogController m_DialogController;

        public override InteractionHelper<PlayerInteractionHelper> InteractionHelper => PlayerInteractionHelper.Instance;

        protected override void HandleInteractionEvent()
        {
            if(FindObjectOfType<DialogController>()) return;
            base.HandleInteractionEvent();
        }
        
        protected override async UniTask HandleInteraction()
        {
            m_DialogController = DialogController.CreateInstanceDialog();
            await m_DialogController.Init(m_DialogueGraph, m_DialogFocusPoint);
        }
    }
}