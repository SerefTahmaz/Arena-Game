using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "Map", menuName = "Game/Map/Map", order = 0)]
    public class MapSO : ScriptableObject
    {
        [SerializeField] private string m_MapName;
        [SerializeField] private Sprite m_PreviewIcon;
        [SerializeField] private int m_RequiredExp;
        [SerializeField] private int m_RewardCurrency;
        [SerializeField] private int m_LoseExp;
        [SerializeField] private int m_LoseCurrency;
        [SerializeField] private string m_SceneName;
        
        public string MapName => m_MapName;
        public Sprite PreviewIcon => m_PreviewIcon;
        public int RequiredExp => m_RequiredExp;
        public int RewardCurrency => m_RewardCurrency;
        public int LoseExp => m_LoseExp;
        public int LoseCurrency => m_LoseCurrency;
        public string SceneName => m_SceneName;
    }
}