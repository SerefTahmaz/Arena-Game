using System;
using DefaultNamespace;
using UnityEngine;

public class cLobbyNameInputAreaController : MonoBehaviour
{
    [SerializeField] private cInputField m_InputField;
    
    private string m_LobbyName = "myLobby";

    public string LobbyName => m_LobbyName;

    private void Awake()
    {
        m_InputField.OnValueChanged.AddListener(OnInput);
    }

    public void OnInput(string lobbyName)
    {
        m_LobbyName = lobbyName;
    }
}