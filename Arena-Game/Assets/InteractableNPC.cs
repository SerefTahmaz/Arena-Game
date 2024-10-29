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

        protected DialogController m_DialogController;

        protected virtual void Start()
        {
            m_PlayerDetector.OnPlayerEntered += HandleOnPlayerEntered;
        }

        private void HandleOnPlayerEntered()
        {
            HandleOnPlayerEnteredAsync();
        }

        protected virtual async UniTask HandleOnPlayerEnteredAsync()
        {
            m_PlayerDetector.IsDetectingPlayer = false;
            m_DialogController = DialogController.CreateInstanceDialog();
            await m_DialogController.Init(m_DialogueGraph, m_DialogFocusPoint);
            await UniTask.WaitForSeconds(0.1f);
            OnDialogEnded();
        }

        protected virtual void OnDialogEnded()
        {
            m_PlayerDetector.IsDetectingPlayer = true;
        }
    }
}