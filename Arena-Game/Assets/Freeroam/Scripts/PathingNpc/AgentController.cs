using System;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace
{
    public class AgentController : MonoBehaviour
    { 
        [SerializeField] private NavMeshAgent m_NavMeshAgent;
        [SerializeField] private NavMeshObstacle m_NavMeshObstacle;

        public NavMeshAgent NavMeshAgent => m_NavMeshAgent;
        
        public bool IsGivingWay { get; set; }

        private void Awake()
        {
            NavMeshAgent.updatePosition = false;
            NavMeshAgent.updateRotation = false;
        }

        public void SetObstacle(bool value)
        {
            m_NavMeshObstacle.gameObject.SetActive(value);
            m_NavMeshAgent.enabled = !value;
            IsGivingWay = value;
        }
    }
}