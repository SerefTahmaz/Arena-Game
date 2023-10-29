using UnityEngine;

public class cLobbyNameInputAreaController : MonoBehaviour
{
    private string m_LobbyName = "myLobby";

    public string LobbyName => m_LobbyName;

    public void OnInput(string lobbyName)
    {
        m_LobbyName = lobbyName;
    }
}