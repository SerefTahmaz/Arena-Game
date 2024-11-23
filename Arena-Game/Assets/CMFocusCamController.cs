using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class CMFocusCamController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private CinemachineTargetGroup m_CinemachineTargetGroup;
    [SerializeField] private CinemachineVirtualCamera m_VirtualCamera;
    
    private cCharacter m_Player => cGameManager.Instance.m_OwnerPlayer;
    public int m_PlayerId => cGameManager.Instance.m_OwnerPlayerId;

    private bool m_IsActive;

    private void Awake()
    {
        CameraManager.Instance.OnCameraChange += CameraChange;
    }

    private void CameraChange(CameraManager.CameraType cameraType)
    {
        if (cameraType == CameraManager.CameraType.Focus)
        {
            m_IsActive = true;
            
            target = FindObjectsOfType<MonoBehaviour>().Where((behaviour => behaviour.TryGetComponent(out IDamagable _)))
                .Where((behaviour =>
                {
                    Debug.Log($"{behaviour.GetComponent<IDamagable>().TeamID}  {m_PlayerId}");
                    
                    return behaviour.GetComponent<IDamagable>().TeamID != m_PlayerId;
                })).Select((behaviour =>behaviour.GetComponent<IDamagable>().FocusPoint )).
                OrderBy((transform1 => Vector3.Distance(m_Player.MovementTransform.position, transform1.position))).FirstOrDefault();

        
            if (target == null)
            {
                CameraManager.Instance.EnableGameplayCam();
            }
            else
            {
                // m_CinemachineVirtualCamera.LookAt = target.transform;

                m_CinemachineTargetGroup.m_Targets[0].target = target;
                m_CinemachineTargetGroup.m_Targets[1].target = m_Player.GetComponent<IDamagable>().FocusPoint;
                m_VirtualCamera.Follow = m_Player.GetComponent<IDamagable>().FocusPoint;
                FocusCharHelper.Instance.Target = target;
            }
        }
        else
        {
            m_IsActive = false;
            FocusCharHelper.Instance.Target = null;
        }
    }
}
