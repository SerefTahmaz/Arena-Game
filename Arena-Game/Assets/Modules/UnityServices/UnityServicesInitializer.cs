using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

public class UnityServicesInitializer : MonoBehaviour
{
    [SerializeField] private cLobbyManager m_LobbyManager;
    [SerializeField] private IAPManager m_IAPManager;

    private void Start()
    {
        Init();
    }

    private async UniTask Init()
    {
        var playerName = "player" + Guid.NewGuid().ToString().Substring(0,20);
        Debug.Log($"{playerName.Length} Player Name {playerName}");
        var options = new InitializationOptions();
        options.SetProfile(playerName);
        options.SetEnvironmentName("production");
// #if UNITY_EDITOR || DEVELOPMENT_BUILD
//             .SetEnvironmentName("test");
// #else
//             .SetEnvironmentName("production");
// #endif
        
        await UnityServices.InitializeAsync(options);
        m_LobbyManager.Init();
        m_IAPManager.Init();
    }
}