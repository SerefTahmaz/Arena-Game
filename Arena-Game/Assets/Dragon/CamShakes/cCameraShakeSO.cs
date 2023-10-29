using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "CamShake", fileName = "CamShake")]
public class cCameraShakeSO : ScriptableObject
{
    public float m_Intensity = 1;
    public float m_Frequency = 1;
    public float m_Duration = 1;
}
