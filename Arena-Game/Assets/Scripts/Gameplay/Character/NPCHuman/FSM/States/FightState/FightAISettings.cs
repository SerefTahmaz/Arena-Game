using UnityEngine;

namespace DefaultNamespace.FightAI
{
    [CreateAssetMenu(fileName = "FightAISettings", menuName = "Game/AI/FightAISettings", order = 0)]
    public class FightAISettings : ScriptableObject
    {
        [SerializeField,Range(0,100)] private int m_AvoidAttackChance;
        [SerializeField,Range(0,100)] private int m_JumpChance;
        [SerializeField] private Vector2 m_AttackDelayRange;
        [SerializeField] private int m_BaseDamage;
        [SerializeField,Range(0,100)] private int m_BuffWithDistanceChance;

        public int AvoidAttackChance => m_AvoidAttackChance;
        public int JumpChance => m_JumpChance;
        public Vector2 AttackDelayRange => m_AttackDelayRange;
        public int BaseDamage => m_BaseDamage;
        public int BuffWithDistanceChance => m_BuffWithDistanceChance;
    }
} 