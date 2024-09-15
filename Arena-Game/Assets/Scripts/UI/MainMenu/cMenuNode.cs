using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.UI;
using UnityEngine;
using UnityEngine.Events;

public class cMenuNode : MonoBehaviour
{
    [SerializeField] private cMenuNode m_ParentNode;
    [SerializeField] private cView m_ChildsView;
    [SerializeField] private UnityEvent m_OnActivateEvent;
    [SerializeField] private UnityEvent m_OnDeActivateEvent;

    public void SetParentNode(cMenuNode menuNode)
    {
        m_ParentNode = menuNode;
    }

    public void Activate()
    {
        if(m_ParentNode != null) m_ParentNode.Deactivate();
        if(m_ChildsView != null) m_ChildsView.Activate();
        m_OnActivateEvent.Invoke();
    }
    
    public void Deactivate()
    {
        if(m_ChildsView != null) m_ChildsView.Deactivate();
        m_OnDeActivateEvent.Invoke();
    }
    
    public void EnableParent()
    {
        if(m_ParentNode != null) m_ParentNode.Activate();
        Deactivate();
    }
}
