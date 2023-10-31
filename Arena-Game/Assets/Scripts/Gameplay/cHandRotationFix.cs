using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cHandRotationFix : MonoBehaviour
{
    [SerializeField] [Range(0,1)] private float m_LerpStartRotation;
    
    private Dictionary<Transform, Quaternion> m_RotDic = new Dictionary<Transform, Quaternion>();

    private List<Transform> m_Transforms = new List<Transform>();

    private void Awake()
    {
        foreach (var VARIABLE in GetComponentsInChildren<Transform>())
        {
            m_Transforms.Add(VARIABLE);
            m_RotDic.Add(VARIABLE, VARIABLE.localRotation);
        }
    }

    private void LateUpdate()
    {
        foreach (var VARIABLE in m_Transforms)
        {
            VARIABLE.localRotation = Quaternion.Slerp(VARIABLE.localRotation, m_RotDic[VARIABLE], m_LerpStartRotation);
        }
    }
}
