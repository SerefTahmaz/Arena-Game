using System;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;
using UnityEngine.AI;

public abstract class cCharacter: MonoBehaviour
{
    [SerializeField] private Transform m_MovementTransform;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private cHealthManager m_HealthManager;
    [SerializeField] private string m_CharacterName;
    [SerializeField] private int m_StartHealth;
    [SerializeField] private cDamageManager m_DamageManager;
    [SerializeField] private int m_TeamId;
    [SerializeField] private NavMeshAgent m_NavMeshAgent;

    public Transform MovementTransform => m_MovementTransform;
    public Animator Animator => m_Animator;
    public cHealthManager HealthManager => m_HealthManager;
    public bool IsHost => CharacterNetworkController.IsHost;

    public abstract cCharacterNetworkController CharacterNetworkController
    {
        get;
    }

    public string CharacterName
    {
        get => m_CharacterName;
        set => m_CharacterName = value;
    }

    public int StartHealth => m_StartHealth;

    public int TeamID => m_TeamId;

    public cDamageManager DamageManager => m_DamageManager;

    public NavMeshAgent MeshAgent => m_NavMeshAgent;

    private void Awake()
    {
        DamageManager.Init(TeamID);
    }
}