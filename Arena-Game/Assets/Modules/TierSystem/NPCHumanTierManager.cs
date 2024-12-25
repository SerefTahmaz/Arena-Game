using System;
using System.Collections.Generic;
using System.Linq;
using ArenaGame.Utils;
using DefaultNamespace.FightAI;
using Gameplay.Character;
using Gameplay.Character.NPCHuman;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{ 
    public class NPCHumanTierManager : MonoBehaviour
    {
        [SerializeField] private List<FightAISettings> m_FightAISettingsList;
        [SerializeField] private NPCHumanFight m_NpcHumanFightState;
        [SerializeField] private HumanCharacter m_HumanCharacter;
        [SerializeField] private List<ArmorSOs> m_ArmorSOsList;

        [Serializable]
        public class ArmorSOs
        {
            public List<ArmorItemSO> m_ArmorItemSos;
        }
        

        private void Awake()
        {
            var playerTier = PlayerTierManager.Instance.CurrentTier;
            playerTier = Mathf.Min(playerTier, m_FightAISettingsList.Count - 1);
            m_NpcHumanFightState.FightAISettings = m_FightAISettingsList[playerTier];
        }

        private void Start()
        {
            var playerTier = PlayerTierManager.Instance.CurrentTier;
            playerTier = Mathf.Clamp(playerTier,0, m_ArmorSOsList.Count - 1);
            
            m_HumanCharacter.SkinManager.ClearAllEquipment();

            var tierArmors = m_ArmorSOsList[playerTier].m_ArmorItemSos;
            
            tierArmors.Shuffle();
            var randomItems = tierArmors.Take(Random.Range(1,5));
            foreach (var armorItemSo in randomItems)
            {
                m_HumanCharacter.SkinManager.EquipItem(armorItemSo);
            }
        }
    }
} 