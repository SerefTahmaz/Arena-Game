using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyMat : MonoBehaviour
{
    [SerializeField] private Renderer m_Source;
    [SerializeField] private Renderer m_Destination;

    [ContextMenu("ChangeMat")]
    public void ChangeMat()
    {
        var mats = m_Source.sharedMaterials;
        m_Destination.sharedMaterials = mats;
    }
}
