using System;
using System.Collections;
using System.Collections.Generic;
using DemoBlast.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteAlways]
public class cCrowdEntity : MonoBehaviour
{
    private void Awake()
    {
        foreach (var VARIABLE in transform.gameObject.GetChilds())
        {
            VARIABLE.gameObject.SetActive(false);
        }
            
        var selected = transform.gameObject.GetChilds().RandomItem();
        selected.gameObject.SetActive(true);

        if (Application.isPlaying)
        {
            selected.GetComponent<Animator>().SetFloat("Offset", Random.value);
        }
    }
}