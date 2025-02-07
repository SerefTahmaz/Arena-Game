﻿using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerCharacter
{
	public class MovementController : MonoBehaviour
	{
		[SerializeField] float m_MovingTurnSpeed = 360;
		[SerializeField] float m_StationaryTurnSpeed = 180;
		[SerializeField] float m_JumpPower = 12f;
		[Range(1f, 4f)][SerializeField] float m_GravityMultiplier = 2f;
		[SerializeField] float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
		[SerializeField] float m_MoveSpeedMultiplier = 1f;
		[SerializeField] float m_AnimSpeedMultiplier = 1f;
		[SerializeField] float m_GroundCheckDistance = 0.1f;
		[SerializeField] private PhysicMaterial m_PhysicMaterial;

		[SerializeField] Rigidbody m_Rigidbody;
		[SerializeField] Animator m_Animator;
		// bool m_IsGrounded;
		float m_OrigGroundCheckDistance;
		const float k_Half = 0.5f;
		float m_TurnAmount;
		float m_ForwardAmount;
		Vector3 m_GroundNormal;
		float m_CapsuleHeight;
		Vector3 m_CapsuleCenter;
		[SerializeField] CapsuleCollider m_Capsule;
		bool m_Crouching;
		private bool m_HasInput = false;

		[SerializeField] private float m_YSpeedThreshold;
		[SerializeField] private CharacterType m_CharacterType;
		
		[Header("Player Step Climb")] 
		[SerializeField] private bool m_DisableStepClimb;
		[SerializeField] private GameObject m_StepRayLower;
		[SerializeField] private float m_StepHeight = .3f;
		[SerializeField] private float m_StepSmooth = 0.1f;
		[SerializeField] private bool m_DebugStepClimb;
		[SerializeField] private float m_StepClimbCastDistance;
		private Vector3 m_MoveInput;
		
		public enum CharacterType
		{
			Ground,
			Fly
		}

		public bool m_EnableFlyingMode;


		void Start()
		{
			m_CapsuleHeight = m_Capsule.height;
			m_CapsuleCenter = m_Capsule.center;

			m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			m_OrigGroundCheckDistance = m_GroundCheckDistance;
		}

		private void Update()
		{
			// if (m_HasInput)
			// {
			// 	m_Capsule.material = null;
			// }
			// else
			// {
			// 	m_Capsule.material = m_PhysicMaterial;
			// }
			
			// Debug.Log($"{transform.name} Root Motion {m_Animator.applyRootMotion}");
			// Move(Vector3.zero);
		} 

		private void FixedUpdate()
		{
			if (!m_DisableStepClimb)
			{
				if(m_DebugStepClimb) Debug.Log(m_MoveInput.magnitude);
				if(m_MoveInput.magnitude > 0) StepClimb();
			}
		}
		private void StepClimb()
		{
			StepClimbRaycast(transform.forward);
			StepClimbRaycast(transform.TransformDirection(1.5f,0,1));
			StepClimbRaycast(transform.TransformDirection(-1.5f,0,1)); 
		}

		private void StepClimbRaycast(Vector3 dir)
		{
			if (Physics.Raycast(m_StepRayLower.transform.position, dir, out var hitLower, m_StepClimbCastDistance+0.1f, Layermask))
			{
				if (!Physics.Raycast(m_StepRayLower.transform.position + Vector3.up*m_StepHeight, dir, out var hitUpper, m_StepClimbCastDistance+0.2f,Layermask))
				{
					m_Rigidbody.position -= new Vector3(0, -m_StepSmooth, 0);
					if(m_DebugStepClimb) Debug.Log("Climbing step!!!!!!!!");
				}
			}
		}

		public void Move(Vector3 move, bool crouch = false, bool jump = false, bool forceValue = false)
		{
			// if (move.magnitude > 0)
			// {
			// 	m_HasInput = true;
			// }
			// else
			// {
			// 	m_HasInput = false;
			// }

			// convert the world relative moveInput vector into a local-relative
			// turn amount and forward amount required to head in the desired
			// direction.
			if (move.magnitude > 1f) move.Normalize();
			move = transform.InverseTransformDirection(move);
			CheckGroundStatus();
			// move = Vector3.ProjectOnPlane(move, m_GroundNormal);
			// move.Normalize();
			m_TurnAmount = Mathf.Atan2(move.x, move.z);
			m_ForwardAmount = move.z;

			m_MoveInput = move;

			ApplyExtraTurnRotation();
			
			// control and velocity handling is different when grounded and airborne:
			HandleGroundedMovement(crouch, jump);

			// ScaleCapsuleForCrouching(crouch);
			PreventStandingInLowHeadroom();

			// send input and other state parameters to the animator
			UpdateAnimator(forceValue);
		}


		void ScaleCapsuleForCrouching(bool crouch)
		{
			if (m_IsGrounded && crouch)
			{
				if (m_Crouching) return;
				m_Capsule.height = m_Capsule.height / 2f;
				m_Capsule.center = m_Capsule.center / 2f;
				m_Crouching = true;
			}
			else
			{
				Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
				float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
				if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
				{
					m_Crouching = true;
					return;
				}
				m_Capsule.height = m_CapsuleHeight;
				m_Capsule.center = m_CapsuleCenter;
				m_Crouching = false;
			}
		}

		void PreventStandingInLowHeadroom()
		{
			// // prevent standing up in crouch-only zones
			// if (!m_Crouching)
			// {
			// 	Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
			// 	float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
			// 	if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
			// 	{
			// 		m_Crouching = true;
			// 	}
			// }
		}


		void UpdateAnimator(bool forceValue)
		{
			// update the animator parameters
			if (!forceValue)
			{
				m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
				m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
			}
			else
			{
				m_Animator.SetFloat("Forward", m_ForwardAmount);
				m_Animator.SetFloat("Turn", m_TurnAmount);
			}
			// m_Animator.SetBool("Crouch", m_Crouching);
			// m_Animator.SetBool("OnGround", m_IsGrounded);
			

			// calculate which leg is behind, so as to leave that leg trailing in the jump animation
			// (This code is reliant on the specific run cycle offset in our animations,
			// and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
			float runCycle =
				Mathf.Repeat(
					m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
			float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;

			m_Animator.speed = m_AnimSpeedMultiplier;
		}


		void HandleAirborneMovement()
		{
			// apply extra gravity from multiplier:
			Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
			m_Rigidbody.AddForce(extraGravityForce);

			// m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
		}


		void HandleGroundedMovement(bool crouch, bool jump)
		{
			// check whether conditions are right to allow a jump:
			if (jump && !crouch && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
			{
				// jump!
				m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
				m_Animator.applyRootMotion = false;
				m_GroundCheckDistance = 0.1f;
			}
		}

		void ApplyExtraTurnRotation()
		{
			// help the character turn faster (this is in addition to root rotation in the animation)
			float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
			transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
		}


		public void OnAnimatorMove()
		{
			switch (m_CharacterType)
			{
				case CharacterType.Ground: 
					if (Time.deltaTime > 0)
					{
						Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;
						
						// we preserve the existing y part of the current velocity.
						v.y = m_Rigidbody.velocity.y;
						m_Rigidbody.velocity = v;
					}
					break;
				case CharacterType.Fly:
					
					var physicVelocity = m_Rigidbody.velocity;
					m_Animator.ApplyBuiltinRootMotion();
					
					//With boolean
					if (!m_EnableFlyingMode)
					{	
						//Direct Set
						// m_Rigidbody.velocity = physicVelocity;
						
						if (m_IsGrounded && Time.deltaTime > 0)
						{
							Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;
						
							// we preserve the existing y part of the current velocity.
							v.y = m_Rigidbody.velocity.y;
							m_Rigidbody.velocity = v;
						}
					}
				
					//With using physics threshold
					//Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;
					// if (!(Mathf.Abs(v.y) > m_YSpeedThreshold))
					// {
					// 	m_Rigidbody.velocity = physicVelocity;
					// }
					
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			
			// we implement this function to override the default root motion.
			// this allows us to modify the positional speed before it's applied.
			// if (m_IsGrounded && Time.deltaTime > 0)
			// {
			// 	Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;
			// 	
			//
			// 	// we preserve the existing y part of the current velocity.
			// 	// v.y = m_Rigidbody.velocity.y;
			// 	m_Rigidbody.velocity = v;
			// 	
			// 	Debug.Log($"Delta posiiton {m_Animator.deltaPosition}");
			// }

			

			

			// if (m_IsGrounded && Time.deltaTime > 0)
			// {
			// 	Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;
			// 	
			//
			// 	// we preserve the existing y part of the current velocity.
			// 	v.y = m_Rigidbody.velocity.y;
			// 	m_Rigidbody.velocity = v;
			// 	
			// 	Debug.Log($"Delta posiiton {m_Animator.deltaPosition}");
			// }
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
			m_Animator.SetFloat("Turn", 0);
			m_Animator.SetFloat("Forward", 0);
		}

		private bool m_IsGrounded;

		public LayerMask Layermask;

		void CheckGroundStatus()
		{
			RaycastHit hitInfo;
#if UNITY_EDITOR
			// helper to visualise the ground check ray in the scene view
			Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
			// 0.1f is a small offset to start the ray from inside the character
			// it is also good to note that the transform position in the sample assets is at the base of the character
			if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance, Layermask))
			{
				m_GroundNormal = hitInfo.normal;
				m_IsGrounded = true;
				// m_Animator.applyRootMotion = true;
			}
			else
			{
				m_IsGrounded = false;
				m_GroundNormal = Vector3.up;
				// m_Animator.applyRootMotion = false;
			}
		}
	}




}