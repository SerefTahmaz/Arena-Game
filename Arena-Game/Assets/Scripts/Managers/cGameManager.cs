using System;
using DefaultNamespace;
using DemoBlast.Managers.SaveManager;
using DemoBlast.Utils;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cGameManager : cSingleton<cGameManager>
{
    [SerializeField] private cPlayerIconList m_PlayerIconList;
    [SerializeField] private cSaveManager m_InstanceSaveManager;
    private ISaveManager m_SaveManager;

    public cPlayerIconList PlayerIconList => m_PlayerIconList;
    public Transform m_OwnerPlayer;
    public int m_OwnerPlayerId;

    public ISaveManager SaveManager
    {
        get
        {
            if (m_SaveManager == null)
            {
                m_SaveManager = m_InstanceSaveManager.GetComponent<ISaveManager>();
            }

            return m_SaveManager;
        }
    }

    private void Awake()
    {
        cPlayerManager.Instance.m_OwnerPlayerSpawn += transform1 =>
        {
            m_OwnerPlayerId = transform1.GetComponent<IDamagable>().TeamID;
            m_OwnerPlayer = transform1;
            CameraManager.Instance.OnPlayerSpawn();
        };
    }

    public async void StartRound(cLevelSO levelSo)
    {
        Debug.Log(levelSo.SceneName);
        await cRelayManager.Instance.CreateRelay();
        NetworkManager.Singleton.SceneManager.LoadScene(levelSo.SceneName, LoadSceneMode.Additive);
    }
}