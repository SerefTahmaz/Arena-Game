using System;
using System.Collections;
using System.Collections.Generic;
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
            CameraManager.Instance.EnableGameplayCam();
        }
    }
}

