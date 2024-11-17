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

public class cRelayManager : cSingleton<cRelayManager>
{
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
            cUIManager.Instance.ShowPage(Page.Loading,this);
            cGameManager.Instance.HandleStartingRelay();
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();
            MultiplayerLocalHelper.instance.NetworkHelper.ResetState();
            cGameManager.Instance.StartGame();
            Debug.Log("GameStarted!!!");
            cUIManager.Instance.HidePage(Page.Loading,this);
            return joinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            return null;
        }
    }
    
    public async void StartSinglePlayer()
    {
        cGameManager.Instance.HandleStartingRelay();
        NetworkManager.Singleton.StartHost();
        MultiplayerLocalHelper.instance.NetworkHelper.ResetState();
        cGameManager.Instance.StartGame();
    }

    [Command]
    public async void JoinRelay(string joinCode)
    {
        try
        {
            cUIManager.Instance.ShowPage(Page.Loading,this);
            cGameManager.Instance.StartGameClient();
            //TODO: make it async
            await MapManager.instance.SetMap(cLobbyManager.Instance.LastMapIndex);
            
            Debug.Log("Joined relay with " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            
            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
            
            Debug.Log($"Last map index {cLobbyManager.Instance.LastMapIndex}");
            cUIManager.Instance.HidePage(Page.Loading,this);
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
}