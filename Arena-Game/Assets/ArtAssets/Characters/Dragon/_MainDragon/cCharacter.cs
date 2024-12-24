using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public abstract class cCharacter: MonoBehaviour
{
    [SerializeField] private Transform m_MovementTransform;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private cHealthManager m_HealthManager;
    [SerializeField] private int m_StartHealth;
    [SerializeField] private cDamageManager m_DamageManager;
    [SerializeField] private int m_TeamId;
    [SerializeField] private NavMeshAgent m_NavMeshAgent;

    public Action<DamageWrapper> OnDamage;

    public Transform MovementTransform => m_MovementTransform;
    public Animator Animator => m_Animator;
    public cHealthManager HealthManager => m_HealthManager;
    public bool IsHost => CharacterNetworkController.IsHost;

    public abstract cCharacterNetworkController CharacterNetworkController
    {
        get;
    }

    public string CharacterName => CharacterNetworkController.PlayerName.Value.ToString();

    public virtual int StartHealth => m_StartHealth;

    public cDamageManager DamageManager => m_DamageManager;

    public NavMeshAgent MeshAgent => m_NavMeshAgent;

    public int TeamID => CharacterNetworkController.m_TeamId.Value;

    public int DefaultTeamId => m_TeamId;
    
    public abstract Action OnActionEnded { get; set; }

    private void Start()
    {
        DamageManager.Init(TeamID);
        CharacterNetworkController.m_TeamId.OnValueChanged += (value, newValue) =>
        {
            DamageManager.UpdateTeamId(TeamID);
        };

        OnActionEnded += () =>
        {
            DamageManager.SetActiveDamage(false);
        };
        
        m_Animator.SetFloat("RandomOffset", Random.RandomRange(0.0f,1.0f));
    }
    
    public void TakeDamage(DamageWrapper damageWrapper)
    {
        OnDamage?.Invoke(damageWrapper);
    }
}