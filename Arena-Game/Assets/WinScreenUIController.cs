using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreenUIController : MonoBehaviour
{
    [SerializeField] private cButton m_ContinueButton;

    private void Awake()
    {
        m_ContinueButton.OnClickEvent.AddListener(HandleContinueClicked);
    }

    private void HandleContinueClicked()
    {
        cGameManager.Instance.HandleWinContinueButton();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
