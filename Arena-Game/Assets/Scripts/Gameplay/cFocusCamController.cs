using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class cFocusCamController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float m_DistanceToPlayer;
    [SerializeField] private float m_Height;
    [SerializeField] private Transform m_Focus;
    [SerializeField] private AnimationCurve m_HeightScaler;
    [SerializeField] private float m_DistanceScaler;
    
    private Transform m_Player => cGameManager.Instance.m_OwnerPlayer.MovementTransform;
    public int m_PlayerId => cGameManager.Instance.m_OwnerPlayerId;
    
    private void Update()
    {
        if (target == null)
        {
            target = FindObjectsOfType<MonoBehaviour>().Where((behaviour => behaviour.TryGetComponent(out IDamagable _)))
                .Where((behaviour =>
                {
                    Debug.Log($"{behaviour.GetComponent<IDamagable>().TeamID}  {m_PlayerId}");
                    
                    return behaviour.GetComponent<IDamagable>().TeamID != m_PlayerId;
                })).Select((behaviour =>behaviour.GetComponent<IDamagable>().FocusPoint )).
                OrderBy((transform1 => Vector3.Distance(m_Player.position, transform1.position))).FirstOrDefault();
        }

        
        if (target == null)
        {
            CameraManager.Instance.EnableGameplayCam();
        }
        else
        {
            // m_CinemachineVirtualCamera.LookAt = target.transform;

            Vector3 dir = m_Player.position - m_Focus.position;
            dir.y = 0;
            Vector3 pos = m_Player.position + dir.normalized * m_DistanceToPlayer;
            pos.y += m_Height*(-m_HeightScaler.Evaluate(m_DistanceScaler*Vector3.Distance(m_Player.position,m_Focus.position)));
            transform.position = pos;
            transform.LookAt(m_Focus, Vector3.up);
            
            m_Focus.position = Vector3.Lerp(m_Focus.position, target.position, Time.deltaTime * 5);
        }
    }
}
