using System;
using DefaultNamespace;
using PlayerCharacter;
using UnityEngine;
using UnityEngine.AI;

public class PathingCharacter: MonoBehaviour
{
    [SerializeField] private Transform m_MovementTransform;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private NavMeshAgent m_NavMeshAgent;
    [SerializeField] private MovementController m_MovementController;
    [SerializeField] private AgentController m_AgentController;
    [SerializeField] private Rigidbody m_Rigidbody; 
 
    public Transform MovementTransform => m_MovementTransform;
    public Animator Animator => m_Animator;
    public NavMeshAgent NavMeshAgent => m_NavMeshAgent;
    public MovementController MovementController => m_MovementController;
    public AgentController AgentController => m_AgentController;
    public Rigidbody Rigidbody => m_Rigidbody;
}