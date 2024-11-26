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
    [SerializeField] private cMenuNode m_MenuNode;
    
    // Start is called before the first frame update
    void Start()
    {
        m_InputField.OnValueChanged.AddListener(HandleOnValueChanged);
        m_MenuNode.OnActivateEvent.AddListener(UpdateUI);
        UpdateUI();
    }

    private void HandleOnValueChanged(string newName)
    {
        ProfileGenerator.SaveProfileName(newName);
    }

    public void UpdateUI()
    {
        m_InputField.SetText(ProfileGenerator.GetPlayerProfile().Name);
    }
}
