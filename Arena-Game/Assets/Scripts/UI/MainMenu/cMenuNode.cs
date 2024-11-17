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

    public UnityEvent OnActivateEvent => m_OnActivateEvent;
    public UnityEvent OnDeActivateEvent => m_OnDeActivateEvent;

    public void SetParentNode(cMenuNode menuNode)
    {
        m_ParentNode = menuNode;
    }

    public void Activate(bool instant = false)
    {
        if(m_ParentNode != null) m_ParentNode.Deactivate(instant);
        if(m_ChildsView != null) m_ChildsView.Activate(instant);
        OnActivateEvent.Invoke();
    }
    
    public void Deactivate(bool instant = false)
    {
        if(m_ChildsView != null) m_ChildsView.Deactivate(instant);
        OnDeActivateEvent.Invoke();
    }
    
    public void Activate()
    {
        Activate(false);
    }
    
    public void Deactivate()
    {
        Deactivate(false);
    }
    
    public void EnableParent(bool instant = false)
    {
        if(m_ParentNode != null) m_ParentNode.Activate(instant);
        Deactivate(instant);
    }
    
    public void EnableParent()
    {
        EnableParent(false);
    }
}
