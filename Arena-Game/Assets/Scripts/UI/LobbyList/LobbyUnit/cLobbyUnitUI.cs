using TMPro;
using UnityEngine;

public class cLobbyUnitUI : MonoBehaviour
{
    [SerializeField] private TMP_Text m_LobbyNameText;
    [SerializeField] private TMP_Text m_PlayerCountText;
    [SerializeField] private TMP_Text m_GameModeText;

    public void UpdateUI(string lobbyNameText,string playerCountText,string gameModeText)
    {
        m_LobbyNameText.text = lobbyNameText;
        m_PlayerCountText.text = playerCountText;
        m_GameModeText.text = gameModeText;
    }
}