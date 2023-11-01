using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class cFocusCamController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float m_DistanceToPlayer;
    [SerializeField] private float m_Height;

    private Transform m_Player => cGameManager.Instance.m_OwnerPlayer;
    public int m_PlayerId => cGameManager.Instance.m_OwnerPlayerId;
    
    private void Update()
    {
        target =FindObjectsOfType<MonoBehaviour>().Where((behaviour => behaviour.TryGetComponent(out IDamagable _)))
        .Where((behaviour => behaviour.GetComponent<IDamagable>().TeamID != m_PlayerId)).Select((behaviour =>behaviour.GetComponent<IDamagable>().FocusPoint )).FirstOrDefault();
        if (target == null)
        {
            CameraManager.Instance.EnableGameplayCam();
        }
        else
        {
            // m_CinemachineVirtualCamera.LookAt = target.transform;

            Vector3 dir = m_Player.position - target.position;
            dir.y = 0;
            Vector3 pos = m_Player.position + dir.normalized * m_DistanceToPlayer;
            pos.y += m_Height;
            transform.position = pos;
            transform.LookAt(target, Vector3.up);
        }
    }
}
