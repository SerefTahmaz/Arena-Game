using System;
using System.Collections;
using System.Collections.Generic;
using DemoBlast.Utils;
using Lean.Touch;
using RootMotion;
using UnityEngine;

public class cJoystickController : cSingleton<cJoystickController>
{
    [SerializeField] private Transform m_Head;
    [SerializeField] private Transform m_HeadMax;
    [SerializeField] private float m_Threshold;

    private bool m_Clicked = false;

    private Vector3 m_MaxDistance;

    private static Vector2 m_JoystickValue;

    public static Vector2 JoystickValue => m_JoystickValue;

    private void Awake()
    {
        m_MaxDistance = m_HeadMax.position - m_Head.position;
    }

    public void OnClick()
    {
        m_Clicked = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            m_Clicked = false;
        }
        
        if (m_Clicked)
        {
            var inputPos = Input.mousePosition;
            var distance = inputPos - transform.position;
            distance = Vector3.ClampMagnitude(distance, m_MaxDistance.magnitude);
            m_Head.position = transform.position + distance;
          
        }
        else
        {
            m_Head.localPosition = Vector3.Lerp(m_Head.localPosition, Vector3.zero, Time.deltaTime * 10);
        }
        
        m_JoystickValue = m_Head.localPosition / m_HeadMax.localPosition.magnitude;
        m_JoystickValue.x = Mathf.Abs(m_JoystickValue.x )> m_Threshold ? m_JoystickValue.x : 0;
        m_JoystickValue.y = Mathf.Abs(m_JoystickValue.y )> m_Threshold ? m_JoystickValue.y : 0;
    }
}
