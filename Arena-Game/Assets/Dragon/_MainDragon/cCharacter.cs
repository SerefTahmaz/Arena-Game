using RootMotion.FinalIK;
using UnityEngine;

public abstract class cCharacter: MonoBehaviour
{
    [SerializeField] private Transform m_MovementTransform;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private cHealthBar m_HealthBar;
    [SerializeField] private string m_CharacterName;
    [SerializeField] private int m_StartHealth;

    public Transform MovementTransform => m_MovementTransform;
    public Animator Animator => m_Animator;
    public cHealthBar HealthBar => m_HealthBar;
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
}