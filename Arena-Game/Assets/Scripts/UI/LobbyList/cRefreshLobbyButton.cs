using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class cRefreshLobbyButton : MonoBehaviour
{
    [SerializeField] private cButton m_Button;

    public void RefreshLobby()
    {
        m_Button.DeActivate();
        cLobbyListUI.Instance.PopulateList(OnRefresh);
    }

    public void OnRefresh()
    {
        Debug.Log("Updated Lobby");
        DOVirtual.DelayedCall(1.5f, () =>
        {
            m_Button.Activate();
        });
    }
}
 