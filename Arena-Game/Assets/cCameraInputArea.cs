using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class cCameraInputArea : MonoBehaviour
{
    private bool m_Selected;
    public void OnFingerDown()
    {
        CameraManager.Instance.StartCameraMovement();
        m_Selected = true;
    }
    
    public void OnFingerUp()
    {
        m_Selected = false;
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DOVirtual.DelayedCall(.01f, () =>
            {
                if (m_Selected == false) CameraManager.Instance.StopCameraMovement();
            });
        }
    }
}
