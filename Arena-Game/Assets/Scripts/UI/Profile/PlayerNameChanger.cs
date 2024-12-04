using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameChanger : MonoBehaviour
{
    [SerializeField] private cInputField m_InputField;
    [SerializeField] private GameObject m_UserNameEmpty;
    [SerializeField] private cMenuNode m_MenuNode;

    private string m_DefaultName;

    private void Awake()
    {
        m_DefaultName = m_InputField.Text;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_InputField.OnValueChanged.AddListener(HandleOnValueChanged);
        m_MenuNode.OnActivateEvent.AddListener(UpdateUI);
        UpdateUI();
    }

    private void HandleOnValueChanged(string newName)
    {
        m_UserNameEmpty.SetActive(false);
        if (string.IsNullOrEmpty(newName))
        {
            m_UserNameEmpty.SetActive(true);
            return;
        }
        
        Debug.Log("User name changed");
        ProfileGenerator.SaveProfileName(newName);
    }

    public void UpdateUI()
    {
        var username = ProfileGenerator.GetPlayerProfile().Name;
        if (string.IsNullOrEmpty(username))
        {
            m_InputField.SetText(m_DefaultName);
            return;
        }
        
        m_InputField.SetText(username);
    }
}
