using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUIController : MonoBehaviour
{
    [SerializeField] private GameObject m_DebugHolder;
    [SerializeField] private Toggle m_Toggle;
      
    // Start is called before the first frame update
    void Start()
    {
        m_Toggle.onValueChanged.AddListener((value =>
        {
            m_DebugHolder.SetActive(value);
        }));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
