using System.Collections;
using System.Collections.Generic;
using DemoBlast.UI;
using UnityEngine;
using UnityEngine.Events;

public class cMenuNode : MonoBehaviour
{
    [SerializeField] private cMenuNode m_ParentNode;
    [SerializeField] private cView m_ChildsView;

    public void Activate()
    {
        if(m_ParentNode != null) m_ParentNode.Deactivate();
        if(m_ChildsView != null) m_ChildsView.Activate();
    }
    
    public void Deactivate()
    {
        if(m_ChildsView != null) m_ChildsView.Deactivate();
    }
    
    public void EnableParent()
    {
        if(m_ParentNode != null) m_ParentNode.Activate();
        if(m_ChildsView != null) m_ChildsView.Deactivate();
    }
}
