using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.ArenaGame.Managers.SaveManager;
using Item;
using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "Character", menuName = "Game/CharacterSSS", order = 0)]
    public class CharacterSO : SerializableScriptableObject
    {
        [SerializeField] private int m_Health;
        [SerializeField] private List<ArmorItem> m_EquipmentList;
        [SerializeField] private List<BaseItemSO> m_InventoryList;

        public Action OnChanged { get; set; }

        public List<ArmorItem> EquipmentList
        {
            get => m_EquipmentList;
            set => m_EquipmentList = value;
        }

        public List<BaseItemSO> InventoryList
        {
            get => m_InventoryList;
            set => m_InventoryList = value;
        }

        public void Save()
        {
            CharacterSaveHandler.Load();
            if (!CharacterSaveHandler.SaveData.Characters.ContainsKey(Guid.ToHexString()))
            {
                CharacterSaveHandler.SaveData.Characters.Add(Guid.ToHexString(), new ArenaGame.Managers.SaveManager.Character());
            }
            
            CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].Health = m_Health;
            CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].EquipmentList = EquipmentList.Select((item => item.Guid.ToHexString())).ToList();
            CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].InventoryList = InventoryList.Select((item => item.Guid.ToHexString())).ToList();
            CharacterSaveHandler.Save();
            OnChanged?.Invoke();
        }

        public void Load()
        {
            CharacterSaveHandler.Load();
            if (CharacterSaveHandler.SaveData.Characters.ContainsKey(Guid.ToHexString()))
            {
                m_Health = CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].Health;
                
                //Convert to items

                LoadEquipmentList();
                LoadInventoryList();
            }
        }

        private void LoadEquipmentList()
        {
            var itemsGuid = CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].EquipmentList;
            var itemsSO = itemsGuid.Select((s => ItemListSO.GetItemByGuid<ArmorItem>(s))).ToList();
            itemsSO.RemoveAll((item => item == null));

            EquipmentList = itemsSO;
        }
        
        private void LoadInventoryList()
        {
            var itemsGuid = CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].InventoryList;
            var itemsSO = itemsGuid.Select((s => ItemListSO.GetItemByGuid<BaseItemSO>(s))).ToList();
            itemsSO.RemoveAll((item => item == null));

            InventoryList = itemsSO;
        }

        public bool IsItemEquiped(BaseItemSO baseItemSo)
        {
            Load();
            return EquipmentList.Contains(baseItemSo);
        }
        
        public bool IsItemInInventory(BaseItemSO baseItemSo)
        {
            Load();
            return InventoryList.Contains(baseItemSo);
        }

        public void EquipItem(ArmorItem item)
        {
            if (IsItemEquiped(item))
            {
                return;
            }
            
            EquipmentList.Add(item);
            Save();
        }

        public void UnequipItem(ArmorItem item)
        {
            if (!IsItemEquiped(item))
            {
                return;
            }
            
            EquipmentList.Remove(item);
            Save();
        }

        public void AddInventory(BaseItemSO item)
        {
            if (IsItemInInventory(item))
            {
                return;
            }
            
            InventoryList.Add(item);
            Save();
        }
    }
}