using System.Collections.Generic;
using System.Linq;
using ArenaGame.Managers.SaveManager;
using DefaultNamespace.ArenaGame.Managers.SaveManager;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "Character", menuName = "Game/Character", order = 0)]
    public class CharacterSO : SerializableScriptableObject
    {
        [SerializeField] private int m_Health;
        [SerializeField] private List<ArmorItem> m_EquipmentList;

        public List<ArmorItem> EquipmentList
        {
            get => m_EquipmentList;
            set => m_EquipmentList = value;
        }

        public void Save()
        {
            CharacterSaveHandler.Load();
            if (!CharacterSaveHandler.SaveData.Characters.ContainsKey(Guid.ToHexString()))
            {
                CharacterSaveHandler.SaveData.Characters.Add(Guid.ToHexString(), new Character());
            }
            
            CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].Health = m_Health;
            CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].EquipmentList = EquipmentList.Select((item => item.Guid.ToHexString())).ToList();
            CharacterSaveHandler.Save();
        }

        public void Load()
        {
            CharacterSaveHandler.Load();
            if (CharacterSaveHandler.SaveData.Characters.ContainsKey(Guid.ToHexString()))
            {
                m_Health = CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].Health;
                
                //Convert to items

                var itemsGuid = CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].EquipmentList;
                var itemsSO = itemsGuid.Select((s => ItemListSO.GetItemByGuid<ArmorItem>(s))).ToList();
                itemsSO.RemoveAll((item => item == null));

                EquipmentList = itemsSO;

            }
        }
    }
}