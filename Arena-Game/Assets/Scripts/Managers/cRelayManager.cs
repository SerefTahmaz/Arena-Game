using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArenaGame.Utils;
using QFSW.QC;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cRelayManager : cSingleton<cRelayManager>
{
    private object m_ClientLoadingLock = new object();
    
    // // Start is called before the first frame update
    // async void Start()
    // {
    //     await UnityServices.InitializeAsync();
    //
    //     AuthenticationService.Instance.SignedIn += () =>
    //     {
    //         
    //     };
    //     await AuthenticationService.Instance.SignInAnonymouslyAsync();
    // }

    [Command]
    public async Task<string> CreateRelay()
    {
        try
        {
            LoadingScreen.Instance.ShowPage(this);
            cGameManager.Instance.HandleStartingRelay();
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();
            NetworkManager.Singleton.SceneManager.SetClientSynchronizationMode(LoadSceneMode.Additive);
            NetworkManager.Singleton.SceneManager.VerifySceneBeforeLoading = SceneVerification;
            
            MultiplayerLocalHelper.Instance.NetworkHelper.ResetState();
            cGameManager.Instance.StartGame();
            Debug.Log("GameStarted!!!");
            LoadingScreen.Instance.HidePage(this);
            return joinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            return null;
        }
    }

    private bool SceneVerification(int sceneindex, string scenename, LoadSceneMode loadscenemode)
    {
        if (scenename == "Main")
        {
            return false;
        }

        return true;
    }

    public async void StartSinglePlayer()
    {
        cGameManager.Instance.HandleStartingRelay();
        NetworkManager.Singleton.StartHost();
        MultiplayerLocalHelper.Instance.NetworkHelper.ResetState();
        cGameManager.Instance.StartGame();
    }

    [Command]
    public async void JoinRelay(string joinCode)
    {
        try
        {
            cGameManager.Instance.HandleStartingRelay();
            LoadingScreen.Instance.ShowPage(this);
            cGameManager.Instance.StartGameClient(true);
            // //TODO: make it async
            await MapManager.Instance.RemoveCurrentLevel();
            
            Debug.Log("Joined relay with " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            
            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
            LoadingScreen.Instance.ShowPage(m_ClientLoadingLock, true);
            NetworkManager.Singleton.SceneManager.OnLoadComplete += SceneManagerOnOnLoadComplete;
            
            Debug.Log($"Last map index {cLobbyManager.Instance.LastMapIndex}");
            LoadingScreen.Instance.HidePage(this);
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

    private void SceneManagerOnOnLoadComplete(ulong clientid, string scenename, LoadSceneMode loadscenemode)
    {
        NetworkManager.Singleton.SceneManager.OnLoadComplete -= SceneManagerOnOnLoadComplete;
        LoadingScreen.Instance.HidePage(m_ClientLoadingLock);
        MapManager.Instance.SetMapIndex(cLobbyManager.Instance.LastMapIndex);
    }
}