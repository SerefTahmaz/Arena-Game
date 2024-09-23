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
        [SerializeField] private List<BaseItemTemplateSO> m_InventoryList;

        [SerializeField] private ArmorItemTemplate m_HelmArmor;
        [SerializeField] private ArmorItemTemplate m_ChestArmor;
        [SerializeField] private ArmorItemTemplate m_GauntletsArmor;
        [SerializeField] private ArmorItemTemplate m_LeggingArmor;

        public Action OnChanged { get; set; }

        public List<BaseItemTemplateSO> InventoryList
        {
            get => m_InventoryList;
            set => m_InventoryList = value;
        }

        public ArmorItemTemplate HelmArmor => m_HelmArmor;
        public ArmorItemTemplate ChestArmor => m_ChestArmor;
        public ArmorItemTemplate GauntletsArmor => m_GauntletsArmor;
        public ArmorItemTemplate LeggingArmor => m_LeggingArmor;

        public int Health => m_Health;

        public void Save()
        {
            CharacterSaveHandler.Load();
            if (!CharacterSaveHandler.SaveData.Characters.ContainsKey(Guid.ToHexString()))
            {
                CharacterSaveHandler.SaveData.Characters.Add(Guid.ToHexString(), new ArenaGame.Managers.SaveManager.Character());
            }
            
            CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].Health = Health;
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
            m_HelmArmor = ItemListSO.GetItemByGuid<ArmorItemTemplate>(CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].HelmArmor);
            m_ChestArmor = ItemListSO.GetItemByGuid<ArmorItemTemplate>(CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].ChestArmor);
            m_GauntletsArmor = ItemListSO.GetItemByGuid<ArmorItemTemplate>(CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].GaunletsArmor);
            m_LeggingArmor = ItemListSO.GetItemByGuid<ArmorItemTemplate>(CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].LeggingArmor);
        }
        
        private void LoadInventoryList()
        {
            var itemsGuid = CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].InventoryList;
            var itemsSO = itemsGuid.Select((s => ItemListSO.GetItemByGuid<BaseItemTemplateSO>(s))).ToList();
            itemsSO.RemoveAll((item => item == null));

            InventoryList = itemsSO;
        }

        public bool IsItemEquiped(BaseItemTemplateSO baseItemTemplateSo)
        {
            Load();

            if (baseItemTemplateSo == ChestArmor || baseItemTemplateSo == HelmArmor || baseItemTemplateSo == GauntletsArmor ||
                baseItemTemplateSo == LeggingArmor) return true;
            else return false;
        }
        
        public bool IsItemInInventory(BaseItemTemplateSO baseItemTemplateSo)
        {
            Load();
            return InventoryList.Contains(baseItemTemplateSo);
        }

        public void EquipItem(ArmorItemTemplate itemTemplate)
        {
            if (IsItemEquiped(itemTemplate))
            {
                return;
            }

            switch (itemTemplate.ArmorType)
            {
                case ArmorType.Helm:
                    m_HelmArmor = itemTemplate;
                    break;
                case ArmorType.Chest:
                    m_ChestArmor = itemTemplate;
                    break;
                case ArmorType.Gauntlets:
                    m_GauntletsArmor = itemTemplate;
                    break;
                case ArmorType.Legging:
                    m_LeggingArmor = itemTemplate;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            Save();
        }

        public void UnequipItem(ArmorItemTemplate itemTemplate)
        {
            if (!IsItemEquiped(itemTemplate))
            {
                Debug.Log("Item not equipped");
                return;
            }
            
            switch (itemTemplate.ArmorType)
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

        public void AddInventory(BaseItemTemplateSO itemTemplate)
        {
            if (IsItemInInventory(itemTemplate))
            {
                return;
            }
            
            InventoryList.Add(itemTemplate);
            Save();
        }

        public void ClearEquipment()
        {
            m_HelmArmor = null;
            m_ChestArmor = null;
            m_GauntletsArmor = null;
            m_LeggingArmor = null;
        }
    }
}