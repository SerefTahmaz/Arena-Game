using Dialogue;
using UnityEngine;

namespace DefaultNamespace
{
    public class InteractableNPC : MonoBehaviour
    {
        [SerializeField] private PlayerDetector m_PlayerDetector;
        [SerializeField] private DialogueGraph m_DialogueGraph;

        private void Start()
        {
            m_PlayerDetector.OnPlayerEntered += HandleOnPlayerEntered;
        }

        private void HandleOnPlayerEntered()
        {
            var ins = DialogController.CreateInstanceDialog(m_DialogueGraph);
        }
    }
}