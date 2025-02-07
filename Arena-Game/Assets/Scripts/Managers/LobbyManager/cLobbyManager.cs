using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArenaGame;
using ArenaGame.Managers.SaveManager;
using ArenaGame.Utils;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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

    public string IconIndex => m_IconIndex;

    public Action m_OnLobbyUpdate= delegate {  };
    
    public int LastMapIndex { get; set; }
    public Action m_OnGameStarted { get; set; }
    
    

    private async void Start()
    {
        m_IconIndex = Random.Range(0, 4).ToString();
        m_PlayerName = "player" + Guid.NewGuid().ToString().Substring(0,20);
        Debug.Log($"{PlayerName.Length} Player Name {PlayerName}");
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
        // if(JoinedLobby == null) Debug.Log("Null lobby");
        // else Debug.Log("Valid lobby");
        
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
                        cLobbyUI.Instance.DisableLobbyUI();
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
                        cLobbyUI.Instance.DisableLobbyUI();
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
    
    // public void CreateLobby(Action onCreated = null)
    // {
    //     CreateLobby("myLobby", 4, false, eGameMode.PvP, onCreated);
    // }

    [Command]
    public async UniTask<RequestResult> CreateLobby(string lobbyName, int maxPlayers, bool isPrivate, eGameMode gameMode)
    {
        try
        {
            UserSaveHandler.Load();
            var map = UserSaveHandler.SaveData.m_CurrentMap;
            
            m_GameStarted = false;
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions()
            {
                IsPrivate = isPrivate,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>()
                {
                    {GAME_MODE, new DataObject(DataObject.VisibilityOptions.Public, gameMode.ToString(), DataObject.IndexOptions.S1)},
                    {KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, "0")},
                    {"Map", new DataObject(DataObject.VisibilityOptions.Public, map.ToString())}
                }
            };
            
            JoinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers,createLobbyOptions);

            Debug.Log($"lobby name :{JoinedLobby.Name} max Player Count:{JoinedLobby.MaxPlayers} lobby id :{JoinedLobby.Id} lobby code:{JoinedLobby.LobbyCode}");
            return RequestResult.Success;
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            return RequestResult.Failed;
        }
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
            LobbyUpdated(JoinedLobby);
            
            onJoined?.Invoke();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            throw;
        }
    }

    private LobbyEventCallbacks m_LobbyEventCallbacks = new LobbyEventCallbacks();
    
    public async UniTask<RequestResult> JoinLobbyById(string lobbyCode)
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
            LobbyUpdated(JoinedLobby);

            await LobbyService.Instance.SubscribeToLobbyEventsAsync(lobby.Id, m_LobbyEventCallbacks);
            m_LobbyEventCallbacks.KickedFromLobby += OnKickedFromLobby;
            
            return RequestResult.Success;
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            return RequestResult.Failed;
        }
    }

    private void OnKickedFromLobby()
    {
        m_LobbyEventCallbacks.KickedFromLobby -= OnKickedFromLobby;
        ClearVariables();
        cLobbyListUI.Instance.ReturnToList();
    }

    private void ClearVariables()
    {
        JoinedLobby = null;
        m_LobbyEventCallbacks = new LobbyEventCallbacks();
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
            { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, ProfileGenerator.GetPlayerProfile().Name) },
            { "IconIndex", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, IconIndex) },
            {"ProfilePhoto",  new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, ProfileGenerator.GetPlayerProfile().ProfilePictureURL)},
            { "IsReady", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, m_IsPlayerReady.ToString()) }
        });
    }

    private void LobbyUpdated(Lobby lobby)
    {
        Debug.Log($"Players in lobby:{lobby.Name} Game Mode: {lobby.Data["GameMode"].Value} Map: {lobby.Data["Map"].Value}");
        foreach (var player in lobby.Players)
        {
            Debug.Log(player.Id +" " + player.Data["PlayerName"].Value);
        }
        
        LastMapIndex = int.Parse(JoinedLobby.Data["Map"].Value);
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
            LobbyUpdated(JoinedLobby);
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
                    { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, ProfileGenerator.GetPlayerProfile().Name)}
                }
            });
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            throw;
        }
    }

    private Tween m_LastIsPlayerReadyUpdate;
    
    [Command]
    public async void UpdateIsPlayerReadyRateLimited(bool ready)
    {
        m_LastIsPlayerReadyUpdate.Kill();
        m_LastIsPlayerReadyUpdate = DOVirtual.DelayedCall(0.5f, () =>
        {
            UpdateIsPlayerReady(ready);
        });
    }
    
    private async void UpdateIsPlayerReady(bool ready)
    {
        try
        {
            m_IsPlayerReady = ready;
            if (JoinedLobby == null) return;

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
    public async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(JoinedLobby.Id, AuthenticationService.Instance.PlayerId);
            ClearVariables();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            throw;
        }
    }
    
    [Command]
    public async UniTask<RequestResult> KickPlayer(string id)
    {
        var token = new object();
        MiniLoadingScreen.Instance.ShowPage(token);
        RequestResult result;
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(JoinedLobby.Id, id);
            result = RequestResult.Success;
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            result = RequestResult.Failed;
        }
        MiniLoadingScreen.Instance.HidePage(token);
        return result;
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
                m_OnGameStarted?.Invoke();

                string relayCode = await cRelayManager.Instance.CreateRelay();
                
                var lobby = await LobbyService.Instance.UpdateLobbyAsync(JoinedLobby.Id, new UpdateLobbyOptions()
                {
                    IsPrivate = true,
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