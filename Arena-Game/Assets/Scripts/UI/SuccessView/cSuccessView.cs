using System.Collections;
using System.Collections.Generic;
using ArenaGame.UI;
using UnityEngine;

public class cSuccessView : MonoBehaviour
{
    [SerializeField] private cView m_View;

    public void Activate()
    {
        m_View.Activate();
    }
    
    public void Deactivate()
    {
        m_View.Deactivate();
    }
}
