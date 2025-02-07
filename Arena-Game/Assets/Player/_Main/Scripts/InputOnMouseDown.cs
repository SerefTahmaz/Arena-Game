﻿using UnityEngine;
using Cinemachine;

public class InputOnMouseDown : MonoBehaviour, AxisState.IInputAxisProvider
{
    public string HorizontalInput = "Mouse X";
    public string VerticalInput = "Mouse Y";

    private bool _disableMovement = true;

    public float GetAxisValue(int axis)
    {
        // Debug.Log(_disableMovement);
        // No input unless right mouse is down
        // if (_disableMovement)
        //     return 0;

#if UNITY_EDITOR
        switch (axis)
        {
            case 0: return Input.GetAxis(HorizontalInput);
            case 1: return Input.GetAxis(VerticalInput);
            default: return 0;
        }
        #else
         switch (axis)
        {
            case 0: return m_JoystickValue.x;
            case 1: return m_JoystickValue.y;
            default: return 0;
        }
#endif
    }

    public void StopCamMovement()
    {
        _disableMovement = true;
    }

    public void StartCamMovement()
    {
        _disableMovement = false;
    }
    private int m_Id = -1;

    public void OnClick()
    {
        if (Input.touchCount > 0 && m_Id == -1)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    m_Id = Input.GetTouch(i).fingerId;
                    return;
                }
            }
        }
    }

    [SerializeField] private float m_FingerInputSpeed;
    
    private Vector2 m_JoystickValue;

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
                // Debug.Log(m_Id);
                clicked = true;
            }
        }


        if (clicked)
        {
            Vector2 input = controllingTouch.deltaPosition;
            input.x /= Screen.width;
            input.y /= Screen.height;
            m_JoystickValue = input*m_FingerInputSpeed;
        }
        else
        {
            m_JoystickValue = Vector2.zero;
        }
    }
}