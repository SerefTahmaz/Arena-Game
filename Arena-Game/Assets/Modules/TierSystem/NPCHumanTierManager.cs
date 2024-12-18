using System;
using System.Collections.Generic;
using DefaultNamespace.FightAI;
using Gameplay.Character.NPCHuman;
using UnityEngine;

namespace DefaultNamespace
{ 
    public class NPCHumanTierManager : MonoBehaviour
    {
        [SerializeField] private List<FightAISettings> m_FightAISettingsList;
        [SerializeField] private NPCHumanFight m_NpcHumanFightState;

        private void Awake()
        {
            var playerTier = PlayerTierManager.Instance.CurrentTier;
            playerTier = Mathf.Min(playerTier, m_FightAISettingsList.Count - 1);
            m_NpcHumanFightState.FightAISettings = m_FightAISettingsList[playerTier];
        }
    }
} 