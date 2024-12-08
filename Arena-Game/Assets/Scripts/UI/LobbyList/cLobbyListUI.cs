using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.UI;
using ArenaGame.Utils;
using QFSW.QC;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Events;

public class cLobbyListUI : cSingleton<cLobbyListUI>
{
    [SerializeField] private cLobbyUnit m_LobbyUnitPrefab;
    [SerializeField] private Transform m_LayoutTransform;
    [SerializeField] private cMenuNode m_MenuNode;

    private void Awake()
    {
        m_MenuNode.OnActivateEvent.AddListener(PopulateList);
    }

    private void PopulateList()
    {
        PopulateList(null);
    }

    [Command]
    public async void PopulateList(Action onUpdated=null)
    {
        try
        {
            foreach (var VARIABLE in m_LayoutTransform.gameObject.GetChilds())
            {
                Destroy(VARIABLE.gameObject);
            }
        
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions()
            {
                Count = 25,
                Filters = new List<QueryFilter>()
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT),
                    new QueryFilter(QueryFilter.FieldOptions.S1, cGameManager.Instance.CurrentGameMode.ToString(), QueryFilter.OpOptions.EQ)
                },
                Order = new List<QueryOrder>(){new QueryOrder(false, QueryOrder.FieldOptions.Created)}
            };
            var queryResponses=await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            foreach (var VARIABLE in queryResponses.Results)
            {
                var ins = Instantiate(m_LobbyUnitPrefab, m_LayoutTransform);
              
                ins.UpdateUI(VARIABLE);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            throw;
        }
        
        onUpdated?.Invoke();
    }

    public void OnCreateLobby()
    {
        m_MenuNode.Deactivate();
    }
 
    public void EnableLobbyListUI()
    {
        m_MenuNode.Activate(true);
    }

    [SerializeField] private UnityEvent m_OnJoined;

    public void OnLobbySelected(Lobby lobby)
    {
        void OnJoined()
        {
            m_OnJoined.Invoke();
        }
        Debug.Log(lobby.LobbyCode);
        cLobbyManager.Instance.JoinLobbyById(lobby.Id, OnJoined);
    }

    public void ReturnToList()
    {
        cLobbyUI.Instance.DisableLobbyUI();
        EnableLobbyListUI();
    }
}
