using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Cinemachine.Manager;
using DG.Tweening;
using UnityEngine;

public class CMFocusCamController : GameCamera
{
    [SerializeField] private CinemachineTargetGroup m_CinemachineTargetGroup;
    [SerializeField] private CinemachineVirtualCamera m_VirtualCamera;
    
    private cCharacter m_Player => cGameManager.Instance.m_OwnerPlayer;
    public int? m_PlayerId => cGameManager.Instance.OwnerPlayerId;

    private bool m_IsActive;
    private IDamagable target;

    public override void Enter()
    {
        base.Enter();
        m_IsActive = true;
        FindATarget();
    }

    public override void Exit()
    {
        base.Exit();
        m_IsActive = false;
        FocusCharHelper.Instance.Target = null;
    }

    private void FindATarget()
    {
        if (m_PlayerId == null)
        {
            CameraManager.Instance.EnableGameplayCam();
            return;
        }
        
        target = FindObjectsOfType<MonoBehaviour>()
            .Where((behaviour => behaviour.TryGetComponent(out IDamagable damagable) && !damagable.IsDead && damagable.TeamID != m_PlayerId))
            .Select((behaviour =>behaviour.GetComponent<IDamagable>() )).
            OrderBy((transform1 => Vector3.Distance(m_Player.MovementTransform.position, transform1.FocusPoint.position))).FirstOrDefault();

        if (target == null)
        {
            CameraManager.Instance.EnableGameplayCam();
        }
        else
        {
            // m_CinemachineVirtualCamera.LookAt = target.transform;

            m_CinemachineTargetGroup.m_Targets[0].target = target.FocusPoint;
            m_CinemachineTargetGroup.m_Targets[1].target = m_Player.GetComponent<IDamagable>().FocusPoint;
            m_VirtualCamera.Follow = m_Player.GetComponent<IDamagable>().FocusPoint;
            FocusCharHelper.Instance.Target = target.FocusPoint;
        }
    }

    public override bool IsAvailable()
    {
        var founded = FindObjectsOfType<MonoBehaviour>()
            .Where((behaviour => behaviour.TryGetComponent(out IDamagable damagable) && !damagable.IsDead && damagable.TeamID != m_PlayerId))
            .Select((behaviour =>behaviour.GetComponent<IDamagable>() )).
            OrderBy((transform1 => Vector3.Distance(m_Player.MovementTransform.position, transform1.FocusPoint.position))).FirstOrDefault();
        return founded != null;
    }

    private void Update()
    {
        if (m_IsActive)
        {
            if (target != null && target.IsDead)
            {
                FindATarget();
            }
        }
    }
}
