using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class cRefreshLobbyButton : MonoBehaviour
{
    [SerializeField] private cButton m_Button;
 
    private void Awake()
    {
        m_Button.OnClickEvent.AddListener(RefreshLobby);
    }

    public void RefreshLobby()
    {
        RefreshLobbyAsync();
    }
    
    private async UniTask RefreshLobbyAsync()
    {
        m_Button.DeActivate();
        await cLobbyListUI.Instance.PopulateListAsync();
        Debug.Log("Updated Lobby");
        await UniTask.WaitForSeconds(1.5f);
        m_Button.Activate();
    }
}
 