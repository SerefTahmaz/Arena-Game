using System.Collections;
using System.Collections.Generic;
using DemoBlast.UI;
using UnityEngine;

public class cScoreboardUIController : MonoBehaviour
{
    [SerializeField] private cView m_ScoreboardTableView;

    public void OnClick()
    {
        if (m_ScoreboardTableView.m_IsActive)
        {
            m_ScoreboardTableView.Deactivate();
        }
        else
        {
            m_ScoreboardTableView.Activate();
        }
    }
}
