using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using Lean.Touch;
using RootMotion;
using UnityEngine;

public class cJoystickController : cSingleton<cJoystickController>
{
    [SerializeField] private Transform m_Head;
    [SerializeField] private Transform m_HeadMax;
    [SerializeField] private float m_Threshold;

    private Vector3 m_MaxDistance;
    private int m_Id = -1;
    private static Vector2 m_JoystickValue;
    
    public static Vector2 JoystickValue => m_JoystickValue;

    private void Awake()
    {
        m_MaxDistance = m_HeadMax.position - m_Head.position;
    }

    public void OnClick()
    {
        if (Input.touchCount > 0)
        {
            m_Id = Input.GetTouch(Input.touchCount-1).fingerId;
        }
    }

    private void Update()
    {
        Touch controllingTouch = new Touch();
        var clicked = false;
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).fingerId == m_Id)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Ended)
                {
                    m_Id = -1;
                    return;
                }
                controllingTouch = Input.GetTouch(i);
                clicked = true;
            }
        }
        
        
        if (clicked)
        {
            var inputPos = new Vector3(controllingTouch.position.x, controllingTouch.position.y,0);
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
