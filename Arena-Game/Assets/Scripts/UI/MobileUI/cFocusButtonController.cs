using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class cFocusButtonController : MonoBehaviour
{
    [SerializeField] private GameObject m_ActiveOverlayIU;

    private void Awake()
    {
        CameraManager.Instance.OnCameraChange += type =>
        {
            if (type == CameraManager.CameraType.Focus)
            {
                m_ActiveOverlayIU.SetActive(true);
            }
            else
            {
                m_ActiveOverlayIU.SetActive(false);
            }
        };
        
        InputManager.Instance.AddListenerToOnFocusCharEvent(HandleFocusCharEvent);
    }

    private void HandleFocusCharEvent()
    {
        if (CameraManager.Instance.CurrentCamType != CameraManager.CameraType.Focus)
        {
            CameraManager.Instance.EnableFocusCam();
        }
        else
        {
            var goCam =CameraManager.Instance.GetCam(CameraManager.CameraType.Gameplay);
            var focusCam =CameraManager.Instance.GetCam(CameraManager.CameraType.Focus);
            var cam =goCam.GetComponent<CinemachineVirtualCameraBase>();
            cam.ForceCameraPosition(focusCam.transform.position, focusCam.transform.rotation);
            CameraManager.Instance.EnableGameplayCam();
        }
    }
}

