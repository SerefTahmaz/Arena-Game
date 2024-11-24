using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using UnityEngine;

public class FocusCharHelper : cSingleton<FocusCharHelper>
{ 
    [SerializeField] private Transform m_Pivot;
    
    public Transform Target { get; set; }

    // Update is called once per frame
    void Update()
    {
        if (Target == null)
        {
            m_Pivot.gameObject.SetActive(false);
            return;
        }
        
        m_Pivot.gameObject.SetActive(true);
        var pos = Camera.main.WorldToScreenPoint(Target.position);
        m_Pivot.transform.position = pos;
    }
}
