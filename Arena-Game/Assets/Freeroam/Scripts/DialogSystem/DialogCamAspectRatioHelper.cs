using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class DialogCamAspectRatioHelper : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera m_VirtualCamera;
    [SerializeField] private AnimationCurve m_MyCurve;
    [SerializeField] private bool m_EnableUpdate;

    private CinemachineComposer m_Composer;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Composer = m_VirtualCamera.GetCinemachineComponent<CinemachineComposer>();
        var aspectRatio = Camera.main.aspect;
        var remap = m_MyCurve.Evaluate(aspectRatio);
        m_Composer.m_TrackedObjectOffset = new Vector3(remap, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_EnableUpdate)
        {
            var aspectRatio = Camera.main.aspect;
            var remap = m_MyCurve.Evaluate(aspectRatio);
            m_Composer.m_TrackedObjectOffset = new Vector3(remap, 0, 0);
        }
    }
}
