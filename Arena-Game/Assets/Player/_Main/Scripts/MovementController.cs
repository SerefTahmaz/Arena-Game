using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Main.Scripts.Gameplay.NPC.States
{
	public class MovementController : MonoBehaviour
{
    [SerializeField] private float _fowardSpeed = 1;
    [SerializeField] private float _rotationSpeed = 1;
    [SerializeField] private float m_StationaryTurnSpeed;
    [SerializeField] private float m_MovingTurnSpeed;
    [SerializeField] float m_GroundCheckDistance = 0.1f;
    
    private float _fowardValue = 0;
    private float _turnValue = 0;
    private Animator _animator;
    Vector3 m_GroundNormal;
    bool m_IsGrounded;
    

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void Move(Vector3 cameraDirection)
    {
        if (cameraDirection.magnitude > 1f) cameraDirection.Normalize();
        cameraDirection = transform.InverseTransformDirection(cameraDirection);
        CheckGroundStatus();
        cameraDirection = Vector3.ProjectOnPlane(cameraDirection, m_GroundNormal);

        _fowardValue = cameraDirection.z;
        
        _turnValue = Mathf.Atan2(cameraDirection.x, cameraDirection.z);
        _animator.SetFloat("Turn", _turnValue * _rotationSpeed, .1f, Time.deltaTime);
        _animator.SetFloat("Foward", cameraDirection.z * _fowardSpeed, .1f, Time.deltaTime);
        
        ApplyExtraTurnRotation();
    }

    public void DisableRotation()
    {
        m_StationaryTurnSpeed = 0;
        m_MovingTurnSpeed = 0;
    }
    
    public void ActivateRotation()
    {
        m_StationaryTurnSpeed = 180;
        m_MovingTurnSpeed = 360;
    }
    
    public void StopMovement()
    {
        _animator.SetFloat("Turn", 0);
        _animator.SetFloat("Foward", 0);
    }
    
    private void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, _fowardValue);
        transform.Rotate(0, _turnValue * turnSpeed * Time.deltaTime, 0);
    }
    
    void CheckGroundStatus()
    		{
    			RaycastHit hitInfo;
    #if UNITY_EDITOR
    			// helper to visualise the ground check ray in the scene view
    			Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
    #endif
    			// 0.1f is a small offset to start the ray from inside the character
    			// it is also good to note that the transform position in the sample assets is at the base of the character
    			if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
    			{
    				m_GroundNormal = hitInfo.normal;
    				m_IsGrounded = true;
                    _animator.applyRootMotion = true;
    			}
    			else
    			{
    				m_IsGrounded = false;
    				m_GroundNormal = Vector3.up;
                    _animator.applyRootMotion = false;
    			}
    		}
}
}

