using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cCreateLobbyButton : MonoBehaviour
{
    [SerializeField] private cCreateLobbyUIController m_CreateLobbyUIController;

    public void CreateLobby()
    {
        cLobbyListUI.Instance.OnCreateLobby();
        Instantiate(m_CreateLobbyUIController, cUIManager.Instance.transform);
    }

    public void OnCreated()
    {
        Debug.Log("Created Lobby");
    }
}
