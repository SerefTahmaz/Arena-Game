using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.ArenaGame.Managers.SaveManager;
using Gameplay.Item;
using Item;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "Character", menuName = "Game/CharacterSSS", order = 0)]
    public class CharacterSO : SerializableScriptableObject
    {
        [SerializeField] private int m_Health;
        [SerializeField] private List<BaseItemSO> m_InventoryList;

        [SerializeField] private ArmorItem m_HelmArmor;
        [SerializeField] private ArmorItem m_ChestArmor;
        [SerializeField] private ArmorItem m_GauntletsArmor;
        [SerializeField] private ArmorItem m_LeggingArmor;

        public Action OnChanged { get; set; }

        public List<BaseItemSO> InventoryList
        {
            get => m_InventoryList;
            set => m_InventoryList = value;
        }

        public ArmorItem HelmArmor => m_HelmArmor;
        public ArmorItem ChestArmor => m_ChestArmor;
        public ArmorItem GauntletsArmor => m_GauntletsArmor;
        public ArmorItem LeggingArmor => m_LeggingArmor;

        public void Save()
        {
            CharacterSaveHandler.Load();
            if (!CharacterSaveHandler.SaveData.Characters.ContainsKey(Guid.ToHexString()))
            {
                CharacterSaveHandler.SaveData.Characters.Add(Guid.ToHexString(), new ArenaGame.Managers.SaveManager.Character());
            }
            
            CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].Health = m_Health;
            CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].InventoryList = InventoryList.Select((item => item.Guid.ToHexString())).ToList();

            CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].HelmArmor = HelmArmor ? HelmArmor.Guid.ToHexString() : "";
            CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].ChestArmor = ChestArmor ? ChestArmor.Guid.ToHexString() : "";
            CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].GaunletsArmor = GauntletsArmor ? GauntletsArmor.Guid.ToHexString() : "";
            CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].LeggingArmor = LeggingArmor ? LeggingArmor.Guid.ToHexString() : "";
            
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
            m_HelmArmor = ItemListSO.GetItemByGuid<ArmorItem>(CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].HelmArmor);
            m_ChestArmor = ItemListSO.GetItemByGuid<ArmorItem>(CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].ChestArmor);
            m_GauntletsArmor = ItemListSO.GetItemByGuid<ArmorItem>(CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].GaunletsArmor);
            m_LeggingArmor = ItemListSO.GetItemByGuid<ArmorItem>(CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].LeggingArmor);
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

            if (baseItemSo == ChestArmor || baseItemSo == HelmArmor || baseItemSo == GauntletsArmor ||
                baseItemSo == LeggingArmor) return true;
            else return false;
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

            switch (item.ArmorType)
            {
                case ArmorType.Helm:
                    m_HelmArmor = item;
                    break;
                case ArmorType.Chest:
                    m_ChestArmor = item;
                    break;
                case ArmorType.Gauntlets:
                    m_GauntletsArmor = item;
                    break;
                case ArmorType.Legging:
                    m_LeggingArmor = item;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            Save();
        }

        public void UnequipItem(ArmorItem item)
        {
            if (!IsItemEquiped(item))
            {
                Debug.Log("Item not equipped");
                return;
            }
            
            switch (item.ArmorType)
            {
                case ArmorType.Helm:
                    m_HelmArmor = null;
                    break;
                case ArmorType.Chest:
                    m_ChestArmor = null;
                    break;
                case ArmorType.Gauntlets:
                    m_GauntletsArmor = null;
                    break;
                case ArmorType.Legging:
                    m_LeggingArmor = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
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