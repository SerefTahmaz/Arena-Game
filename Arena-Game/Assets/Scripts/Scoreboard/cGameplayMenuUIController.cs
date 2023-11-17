using System.Collections;
using System.Collections.Generic;
using DemoBlast.UI;
using UnityEngine;

public class cGameplayMenuUIController : MonoBehaviour
{
    [SerializeField] private cView m_MenuView;

    public void OnClick()
    {
        if (m_MenuView.m_IsActive)
        {
            m_MenuView.Deactivate();
        }
        else
        {
            m_MenuView.Activate();
        }
    }

    public void DeactivateView()
    {
        m_MenuView.Deactivate();
    }
}
