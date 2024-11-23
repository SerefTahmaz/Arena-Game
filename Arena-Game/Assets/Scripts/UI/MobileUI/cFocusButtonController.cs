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
    [SerializeField] private float m_YOffset;
    [SerializeField] private float m_ScaleAmount;
    [SerializeField] private float m_FocusYOffset;
    [SerializeField] private float m_FocusScaleAmount;

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
            
            Debug.Log($"Cam changed {type}");
        };
        
        InputManager.Instance.AddListenerToOnFocusCharEvent(HandleFocusCharEvent);
    }

    private void HandleFocusCharEvent()
    {
        if (CameraManager.Instance.CurrentCamType != CameraManager.CameraType.Focus)
        {
            CopyTransformTo(CameraManager.CameraType.Gameplay,CameraManager.CameraType.Focus, m_FocusScaleAmount, m_FocusYOffset);
            CameraManager.Instance.EnableFocusCam();
        }
        else
        {
            CopyTransformTo(CameraManager.CameraType.Focus, CameraManager.CameraType.Gameplay, m_ScaleAmount, m_YOffset);
            CameraManager.Instance.EnableGameplayCam();

            // var focus = goCam.GetComponent<CinemachineFreeLook>();
            // focus.m_Orbits[1].
        }
    }

    private void CopyTransformTo(CameraManager.CameraType sourceCam, CameraManager.CameraType targetCam, float scale, float yOffset)
    {
        var goCam =CameraManager.Instance.GetCam(targetCam);
        var focusCam =CameraManager.Instance.GetCam(sourceCam);
        var cam =goCam.GetComponent<CinemachineVirtualCameraBase>();
        var localPos = 
            cGameManager.Instance.m_OwnerPlayer.MovementTransform.InverseTransformPoint(focusCam.transform.position);
        var worldPos =  cGameManager.Instance.m_OwnerPlayer.MovementTransform.TransformPoint(localPos*scale) + Vector3.up*yOffset;
            
        cam.ForceCameraPosition(worldPos, focusCam.transform.rotation);
    }
}

