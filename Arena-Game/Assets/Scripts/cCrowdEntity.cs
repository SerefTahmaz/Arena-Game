using System;
using System.Collections;
using System.Collections.Generic;
using DemoBlast.Utils;
using FSG.MeshAnimator;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteAlways]
public class cCrowdEntity : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_Childs;
    
    private void Start()
    {
        foreach (var VARIABLE in m_Childs)
        {
            VARIABLE.gameObject.SetActive(false);
        }
            
        var selected = m_Childs.RandomItem();
        selected.gameObject.SetActive(true);

        if (Application.isPlaying)
        {
            selected.GetComponent<MeshAnimatorBase>().SetTimeNormalized(Random.value, true);
        }
    }
}