using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class cScoreboardUIUnitController : MonoBehaviour
{
    [SerializeField] private TMP_Text m_NameText;
    [SerializeField] private TMP_Text m_KillText;
    [SerializeField] private TMP_Text m_DeadText;
    [SerializeField] private Image m_Icon;

    public void Init(string playerName, int killCount, int deadCount, int iconIndex)
    {
        m_NameText.text = playerName;
        m_KillText.text = killCount.ToString();
        m_DeadText.text = deadCount.ToString();
        m_Icon.sprite = cGameManager.Instance.PlayerIconList.PlayerIcons[iconIndex].Icon;
    }
}
