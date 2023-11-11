using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cDragonAnimationEvents : MonoBehaviour
{
    public Action m_OnDragonShoutEnd = delegate {  };
    
    public void OnShoutEnd()
    {
        m_OnDragonShoutEnd.Invoke();
    }
}
