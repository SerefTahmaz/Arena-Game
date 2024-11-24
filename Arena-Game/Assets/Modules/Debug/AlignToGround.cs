using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class AlignToGround : MonoBehaviour
{
    [SerializeField] private bool m_Align;
    [SerializeField] private LayerMask m_LayerMask;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Align)
        {
            var castPos = transform.position + Vector3.up*10;
            if (Physics.Raycast(castPos, Vector3.down, out var hit, 50,m_LayerMask,QueryTriggerInteraction.Ignore))
            {
                transform.up = hit.normal;
                transform.position = hit.point;
            }
        }
    }

    private void OnDestroy()
    {
        transform.Rotate(0,Random.Range(-180.0f,180.0f),0);
        transform.localScale *= Random.Range(0.8f, 1.2f);
    }
}
