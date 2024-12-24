using UnityEngine;

[CreateAssetMenu(fileName = "PVELevel", menuName = "Game/PVELevel/Level")]
public class PVELevelSO : ScriptableObject
{
    [SerializeField] private Sprite m_Icon;
    [SerializeField,Range(0,1)] private float m_Alpha=1;
    [SerializeField] private string m_NameText;
    [SerializeField] private string m_SceneName;
    [SerializeField] private int m_ExpReward;

    public Sprite Icon => m_Icon;
    public float Alpha => m_Alpha;
    public string NameText => m_NameText;
    public string SceneName => m_SceneName;
    public int ExpReward => m_ExpReward;
}
