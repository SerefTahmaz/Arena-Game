using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class cNetworkController : MonoBehaviour
{
    [SerializeField] private Button m_Host;
    [SerializeField] private Button m_Server;
    [SerializeField] private Button m_Client;

    private void Awake()
    {
        m_Host.onClick.AddListener((() =>
        {
            NetworkManager.Singleton.StartHost();
        }));
        m_Server.onClick.AddListener((() =>
        {
            NetworkManager.Singleton.StartServer();
        }));
        m_Client.onClick.AddListener((() =>
        {
            NetworkManager.Singleton.StartClient();
        }));
    }
}