using System.Collections;
using System.Collections.Generic;
using PlayerCharacter;
using UnityEngine;
using UnityEngine.AI;

public class PointFollower : MonoBehaviour
{
    [SerializeField] private Transform m_Target;
    [SerializeField] private Transform m_MovementTransform;
    [SerializeField] private float m_StopDist;
    [SerializeField] private MovementController m_MovementController;
    [SerializeField] private NavMeshAgent m_NavMeshAgent;
    [SerializeField] private int m_Index;
    [SerializeField] private float m_OffsetDistance;
    
    
    // Start is called before the first frame update
    void Start()
    {
        m_NavMeshAgent.updatePosition = false;
        m_NavMeshAgent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        var path = new NavMeshPath();
        var targetPoint = m_NavMeshAgent.steeringTarget;
      
        
        Vector3 dir = targetPoint - m_MovementTransform.position;
        dir.y = 0;
        dir.Normalize();
        Vector3 movementVector = m_MovementTransform.forward;
        movementVector.y = 0;
        var angle = Vector3.SignedAngle(movementVector, dir, Vector3.up);
        

        if (Vector3.Distance(m_MovementTransform.position, m_Target.position) > m_StopDist)
        {
            m_NavMeshAgent.SetDestination(m_Target.position);
            m_MovementController.Move(dir);
        }
        else
        {
            // m_NavMeshAgent.isStopped = true;
            m_MovementController.Move(Vector3.zero);
        }

        m_NavMeshAgent.nextPosition = transform.position;
    }
}
