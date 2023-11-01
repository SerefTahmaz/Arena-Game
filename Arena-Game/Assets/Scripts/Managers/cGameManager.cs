using System;
using DefaultNamespace;
using DemoBlast.Utils;
using UnityEngine;

public class cGameManager : cSingleton<cGameManager>
{
    [SerializeField] private cPlayerIconList m_PlayerIconList;

    public cPlayerIconList PlayerIconList => m_PlayerIconList;

    public Transform m_OwnerPlayer;
    public int m_OwnerPlayerId;

    private void Awake()
    {
        cPlayerManager.Instance.m_OwnerPlayerSpawn += transform1 =>
        {
            m_OwnerPlayerId = transform1.GetComponent<IDamagable>().TeamID;
            m_OwnerPlayer = transform1;
            CameraManager.Instance.OnPlayerSpawn();
        };
    }
}