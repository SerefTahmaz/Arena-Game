using System;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace
{
    public class AgentController : MonoBehaviour
    { 
        [SerializeField] private NavMeshAgent m_NavMeshAgent;

        public NavMeshAgent NavMeshAgent => m_NavMeshAgent;

        private void Awake()
        {
            NavMeshAgent.updatePosition = false;
            NavMeshAgent.updateRotation = false;
        }
    }
}