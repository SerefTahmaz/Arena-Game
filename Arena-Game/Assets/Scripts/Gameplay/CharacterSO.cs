using System;
using System.Collections.Generic;
using System.Linq;
using ArenaGame.Managers.SaveManager;
using Authentication;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Firebase.Database;
using Gameplay.Item;
using Item;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "Character", menuName = "Game/CharacterSSS", order = 0)]
    public class CharacterSO : SerializableScriptableObject
    { 
        [SerializeField] private int m_StartHealth;
        [SerializeField] private ArmorItemSO m_StartLeggingArmorSO;
        [SerializeField] private ArmorItemSO m_StartGauntletsArmorSO;
        [SerializeField] private ArmorItemSO m_StartChestArmorSO;
        [SerializeField] private ArmorItemSO m_StartHelmArmorSO;
        [SerializeField] private List<BaseItemSO> m_StartInventory;
        

        public int StartHealth => m_StartHealth;
        public ArmorItemSO StartHelmArmorSo => m_StartHelmArmorSO;
        public ArmorItemSO StartChestArmorSo => m_StartChestArmorSO;
        public ArmorItemSO StartGauntletsArmorSo => m_StartGauntletsArmorSO;
        public ArmorItemSO StartLeggingArmorSo => m_StartLeggingArmorSO;
        public List<BaseItemSO> StartInventory => m_StartInventory;

        public CharacterSaveHandler GetCharacterSave()
        {
            if (!CharacterSaveManager.Instance) return null;
            return CharacterSaveManager.Instance.GetController(Guid.ToHexString(),this);
        }
    }
    
   
}