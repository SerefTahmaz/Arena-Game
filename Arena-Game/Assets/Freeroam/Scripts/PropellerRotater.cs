using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PropellerRotater : MonoBehaviour
{
    [SerializeField] private List<Transform> m_RotatePivots;
    [SerializeField] private float m_Speed;
    [SerializeField] private Vector3 m_Axis;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var properler in m_RotatePivots)
        {
            properler.localEulerAngles += m_Axis * m_Speed * Time.deltaTime;
        }
    }
}
