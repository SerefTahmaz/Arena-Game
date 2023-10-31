using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DemoBlast.Utils;
using QFSW.QC;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Random = UnityEngine.Random;

public class cLobbyManager : cSingleton<cLobbyManager>
{
    private Lobby m_JoinedLobby;
    private float m_HearbeatTimer=15;
    private float m_LobbyUpdateTimer=2.2f;
    private string m_PlayerName;
    private string m_IconIndex;
    private bool m_IsPlayerReady;

    public Lobby JoinedLobby
    {
        get => m_JoinedLobby;
        private set => m_JoinedLobby = value;
    }

    public bool IsHost => JoinedLobby != null && JoinedLobby.HostId == AuthenticationService.Instance.PlayerId;

    public string PlayerName => m_PlayerName;

    public Action m_OnLobbyUpdate= delegate {  };
    
    

    private async void Start()
    {
        m_IconIndex = Random.Range(0, 4).ToString();
        m_PlayerName = "player" + Random.Range(10, 99);
        var options = new InitializationOptions();
        options.SetProfile(PlayerName);
        await UnityServices.InitializeAsync(options);

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log($"Signed in {AuthenticationService.Instance.PlayerId}");
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Update()
    {
        LobbyHeartbeat();
        HandleLobbyPollForUpdates();
    }

    private async void LobbyHeartbeat()
    {
        if (IsHost)
        {
            m_HearbeatTimer -= Time.deltaTime;
            if (m_HearbeatTimer <= 0)
            {
                m_HearbeatTimer = 30;

                await LobbyService.Instance.SendHeartbeatPingAsync(JoinedLobby.Id);
            }
        }
    }

    private bool m_GameStarted = false;
    
    private async void HandleLobbyPollForUpdates()
    {
        if (JoinedLobby != null)
        {
            m_LobbyUpdateTimer -= Time.deltaTime;
            if (m_LobbyUpdateTimer <= 0)
            {
                m_LobbyUpdateTimer = 2.2f;

                try
                {
                    JoinedLobby = await LobbyService.Instance.GetLobbyAsync(JoinedLobby.Id);
                
                    if (JoinedLobby.Players.Any((player => player.Id == AuthenticationService.Instance.PlayerId)))
                    {
                        m_OnLobbyUpdate.Invoke();
                    }
                    else if(cLobbyUI.Instance.m_Active)
                    {
                        cLobbyUI.Instance.DisableLobby();
                        cLobbyListUI.Instance.EnableLobbyListUI();
                        JoinedLobby = null;
                    }

                    if (JoinedLobby.Data[KEY_START_GAME].Value != "0")
                    {
                        if (IsHost == false)
                        {
                            cRelayManager.Instance.JoinRelay(JoinedLobby.Data[KEY_START_GAME].Value);
                        }

                        JoinedLobby = null;
                        m_GameStarted = true;
                        cLobbyUI.Instance.DisableLobby();
                    }
                    
                    if (IsHost&&m_GameStarted== false)
                    {
                        bool startGame=true;
                        foreach (var VARIABLE in JoinedLobby.Players)
                        {
                            if (VARIABLE.Data["IsReady"].Value == "False")
                            {
                                startGame = false;
                            }
                        }

                        if (startGame)
                        {
                            StartGame();
                            m_GameStarted = true;
                        }
                    }
                }
                catch (LobbyServiceException e)
                {
                    JoinedLobby = null;
                    Debug.Log(e);
                }
            }
        }
    }

    private const string KEY_START_GAME = "KeyStartGame";
    private const string GAME_MODE = "GameMode";
    
    public enum Visibility
    {
        Private,
        Public
    }
    
    public enum GameMode
    {
        PvP,
        PvE
    }


    public void CreateLobby(Action onCreated = null)
    {
        CreateLobby("myLobby", 4, false, GameMode.PvP, onCreated);
    }

    [Command]
    public async void CreateLobby(string lobbyName, int maxPlayers, bool isPrivate, GameMode gameMode, Action onCreated = null)
    {
        try
        {
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions()
            {
                IsPrivate = isPrivate,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>()
                {
                    {GAME_MODE, new DataObject(DataObject.VisibilityOptions.Public, gameMode.ToString())},
                    {KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, "0")},
                    {"Map", new DataObject(DataObject.VisibilityOptions.Public, "de_dust2")}
                }
            };
            
            JoinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers,createLobbyOptions);

            Debug.Log($"lobby name :{JoinedLobby.Name} max Player Count:{JoinedLobby.MaxPlayers} lobby id :{JoinedLobby.Id} lobby code:{JoinedLobby.LobbyCode}");
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            throw;
        }
        
        onCreated?.Invoke();
    }

    [Command]
    public async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions()
            {
                Count = 25,
                Filters = new List<QueryFilter>()
                    { new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT) },
                Order = new List<QueryOrder>(){new QueryOrder(false, QueryOrder.FieldOptions.Created)}
            };
            
            var queryLobbies = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);
            Debug.Log($"Founded lobbies:{queryLobbies.Results.Count}");
            foreach (var lobby in queryLobbies.Results)
            {
                Debug.Log($"lobby name :{lobby.Name} max Player Count:{lobby.MaxPlayers} Game Mode: {lobby.Data["GameMode"].Value}");
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            throw;
        }
    }

    [Command]
    public async void JoinLobby(string lobbyCode, Action onJoined = null)
    {
        try
        {
            // var queryLobbies = await Lobbies.Instance.QueryLobbiesAsync();
            // Debug.Log($"Founded lobbies:{queryLobbies.Results.Count}");
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions()
            {
                Player = GetPlayer()
            };

            var lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);
            JoinedLobby = lobby;
            Debug.Log("Joined Lobby with code " + lobbyCode);
            PrintPlayers(JoinedLobby);
            
            onJoined?.Invoke();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            throw;
        }
    }
    
    public async void JoinLobbyById(string lobbyCode, Action onJoined = null)
    {
        try
        {
            // var queryLobbies = await Lobbies.Instance.QueryLobbiesAsync();
            // Debug.Log($"Founded lobbies:{queryLobbies.Results.Count}");
            JoinLobbyByIdOptions joinLobbyByCodeOptions = new JoinLobbyByIdOptions()
            {
                Player = GetPlayer()
            };

            var lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyCode, joinLobbyByCodeOptions);
            JoinedLobby = lobby;
            Debug.Log("Joined Lobby with code " + lobbyCode);
            PrintPlayers(JoinedLobby);
            
            onJoined?.Invoke();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            throw;
        }
    }

    [Command]
    private async void QuickJoinLobby()
    {
        try
        {
            QuickJoinLobbyOptions quickJoinLobbyOptions = new QuickJoinLobbyOptions()
            {
                Player = GetPlayer()
            };
            JoinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync(quickJoinLobbyOptions);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            throw;
        }
    }

    private Player GetPlayer()
    {
        return new Player(data: new Dictionary<string, PlayerDataObject>()
        {
            { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, PlayerName) },
            { "IconIndex", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, m_IconIndex) },
            { "IsReady", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, m_IsPlayerReady.ToString()) }
        });
    }

    [Command]
    private void PrintPlayers()
    {
        PrintPlayers(JoinedLobby);
    }

    private void PrintPlayers(Lobby lobby)
    {
        Debug.Log($"Players in lobby:{lobby.Name} Game Mode: {lobby.Data["GameMode"].Value} Map: {lobby.Data["Map"].Value}");
        foreach (var player in lobby.Players)
        {
            Debug.Log(player.Id +" " + player.Data["PlayerName"].Value);
        }
    }

    [Command]
    private async void UpdateLobbyGameMode(string gameMode)
    {
        try
        {
            JoinedLobby = await LobbyService.Instance.UpdateLobbyAsync(JoinedLobby.Id, new UpdateLobbyOptions()
            {
                Data = new Dictionary<string, DataObject>()
                {
                    { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, gameMode) }
                }
            });
            PrintPlayers(JoinedLobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            throw;
        }
    }

    [Command]
    private async void UpdatePlayerName(string newPlayerName)
    {
        try
        {
            m_PlayerName = newPlayerName;
            await LobbyService.Instance.UpdatePlayerAsync(JoinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions()
            {
                Data = new Dictionary<string, PlayerDataObject>()
                {
                    { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, PlayerName)}
                }
            });
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            throw;
        }
    }
    
    [Command]
    public async void UpdateIsPlayerReady(bool ready)
    {
        try
        {
            m_IsPlayerReady = ready;
            await LobbyService.Instance.UpdatePlayerAsync(JoinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions()
            {
                Data = new Dictionary<string, PlayerDataObject>()
                {
                    { "IsReady", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, m_IsPlayerReady.ToString())}
                }
            });
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            throw;
        }
    }
    
    [Command]
    private async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(JoinedLobby.Id, AuthenticationService.Instance.PlayerId);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            throw;
        }
    }
    
    [Command]
    public async void KickPlayer(string id)
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(JoinedLobby.Id, id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            throw;
        }
    }

    private async void MigrateLobby()
    {
        try
        {
            JoinedLobby = await LobbyService.Instance.UpdateLobbyAsync(JoinedLobby.Id, new UpdateLobbyOptions()
            {
                HostId = JoinedLobby.Players[1].Id
            });
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            throw;
        }
    }

    private async void DeleteLobby()
    {
        try
        {
            await LobbyService.Instance.DeleteLobbyAsync(JoinedLobby.Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            throw;
        }
    }

    public async void StartGame()
    {
        if (IsHost)
        {
            try
            {
                Debug.Log("Start Game");

                string relayCode = await cRelayManager.Instance.CreateRelay();
                
                var lobby = await LobbyService.Instance.UpdateLobbyAsync(JoinedLobby.Id, new UpdateLobbyOptions()
                {
                    Data = new Dictionary<string, DataObject>()
                    {
                        { KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, relayCode) }
                    }
                });

                JoinedLobby = lobby;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }
}