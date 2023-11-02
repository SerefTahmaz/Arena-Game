using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;

public class cFocusButtonController : MonoBehaviour
{
    [SerializeField] private GameObject m_IsFocusCamActive;
    

    private void Awake()
    {
        CameraManager.Instance.OnCameraChange += type =>
        {
            if (type == CameraManager.CameraType.Focus)
            {
                m_IsFocusCamActive.SetActive(true);
            }
            else
            {
                m_IsFocusCamActive.SetActive(false);
            }
        };
    }

    public void OnClick()
    {
        if (CameraManager.Instance.CurrentCam != CameraManager.CameraType.Focus)
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

