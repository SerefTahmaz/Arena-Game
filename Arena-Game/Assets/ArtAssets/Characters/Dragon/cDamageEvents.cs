using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cDamageEvents : MonoBehaviour
{
    public Action m_OnDamageStart= delegate {  };
    public Action m_OnDamageEnd= delegate {  };
    public Action m_OnDamageStartLeft= delegate {  };
    public Action m_OnDamageEndLeft= delegate {  };
    
    public void DamageStart()
    {
        m_OnDamageStart.Invoke();
    }
    
    public void DamageEnd()
    {
        m_OnDamageEnd.Invoke();
    }
    
    public void DamageStartLeft()
    {
        m_OnDamageStartLeft.Invoke();
    }
    
    public void DamageEndLeft()
    {
        m_OnDamageEndLeft.Invoke();
    }
}
