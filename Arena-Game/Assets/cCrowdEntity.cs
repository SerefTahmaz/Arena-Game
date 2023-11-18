using System;
using System.Collections;
using System.Collections.Generic;
using DemoBlast.Utils;
using UnityEngine;

[ExecuteAlways]
public class cCrowdEntity : MonoBehaviour
{
    private void Awake()
    {
        foreach (var VARIABLE in transform.gameObject.GetChilds())
        {
            VARIABLE.gameObject.SetActive(false);
        }
            
        transform.gameObject.GetChilds().RandomItem().gameObject.SetActive(true);
    }
}