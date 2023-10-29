using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;

public class cLateParentConstraint : MonoBehaviour
{
    [SerializeField] private Transform m_Target;
    [SerializeField] private LookAtIK m_LookAtIK;

    private Vector3 startPos;
    private Quaternion startRot;

    private void Start()
    {
        startPos = transform.position-m_Target.position;
        startRot = Quaternion.Inverse(m_Target.rotation)*transform.rotation;

        if (m_LookAtIK)
        {
            m_LookAtIK.solver.OnPostUpdate += () =>
            {
                transform.position = startPos + m_Target.position;
                transform.rotation = m_Target.rotation * startRot;
            };
        }
    }

    private void LateUpdate()
    {
        transform.position = startPos + m_Target.position;
        transform.rotation = m_Target.rotation * startRot;
    }
}
