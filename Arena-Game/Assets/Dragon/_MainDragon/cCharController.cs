using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cCharController : MonoBehaviour
{
    private bool m_InputRecieved = false;

    private bool m_CanInputRecieve = false;

    [SerializeField] private Animator m_Animator;

    [SerializeField] private GameObject m_Blood;

    private bool m_IsAnimating = false;

    public void InputStartToRecieve()
    {
        m_CanInputRecieve = true;
        m_InputRecieved = false;
        
        m_Blood.SetActive(false);
    }
    
    public void InputeEndToRecieve()
    {
        if (m_InputRecieved == false)
        {
            m_Animator.SetTrigger("EmptyAttack");
            OnEnd();
        }
        
        m_CanInputRecieve = false;
        
        m_Blood.SetActive(true);
    }

    public void OnEnd()
    {
        m_Animator.applyRootMotion = false;
        m_IsAnimating = false;
        
        m_Blood.SetActive(false);
    }
    
    public void OnStart()
    {
        m_Animator.applyRootMotion = true;
        m_IsAnimating = true;
        
        m_Blood.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && m_CanInputRecieve)
        {
            m_InputRecieved = true;
        }
        
        if (Input.GetMouseButtonDown(0) && m_IsAnimating == false)
        {
            m_Animator.SetTrigger("Attack");
        }
    }
}
