using System;
using System.Collections.Generic;
using System.Linq;
using ArenaGame.Managers.SaveManager;
using DefaultNamespace;
using Gameplay.Item;
using Item;
using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "Character", menuName = "Game/CharacterSSS", order = 0)]
    public class CharacterSO : SerializableScriptableObject
    {
        [SerializeField] private int m_Health;
        [SerializeField] private int m_Currency;
        [SerializeField] private List<BaseItemSO> m_InventoryList;

        [SerializeField] private ArmorItemSO m_HelmArmor;
        [SerializeField] private ArmorItemSO m_ChestArmor;
        [SerializeField] private ArmorItemSO m_GauntletsArmor;
        [SerializeField] private ArmorItemSO m_LeggingArmor;

        public Action OnChanged { get; set; }

        public List<BaseItemSO> InventoryList
        {
            get => m_InventoryList;
            set => m_InventoryList = value;
        }

        public ArmorItemSO HelmArmor => m_HelmArmor;
        public ArmorItemSO ChestArmor => m_ChestArmor;
        public ArmorItemSO GauntletsArmor => m_GauntletsArmor;
        public ArmorItemSO LeggingArmor => m_LeggingArmor;

        public int Health => m_Health;
        public int Currency => m_Currency;

        public void Save()
        {
            CharacterSaveHandler.Load();
            if (!CharacterSaveHandler.SaveData.Characters.ContainsKey(Guid.ToHexString()))
            {
                CharacterSaveHandler.SaveData.Characters.Add(Guid.ToHexString(), new ArenaGame.Managers.SaveManager.Character());
            }
            
            CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].Health = Health;
            CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].Currency = Currency;
            CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].InventoryList = InventoryList.Select((item =>
            {
                item.Save();
                return item.Guid.ToHexString();
            })).ToList();

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
                m_Currency = CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].Currency;
                //Convert to items

                LoadEquipmentList();
                LoadInventoryList();
            }
        }

        private void LoadEquipmentList()
        {
            m_HelmArmor = ItemSaveHandler.GetArmorItem(CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].HelmArmor);
            m_ChestArmor = ItemSaveHandler.GetArmorItem(CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].ChestArmor);
            m_GauntletsArmor = ItemSaveHandler.GetArmorItem(CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].GaunletsArmor);
            m_LeggingArmor = ItemSaveHandler.GetArmorItem(CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].LeggingArmor);
        }
        
        private void LoadInventoryList()
        {
            var itemsGuid = CharacterSaveHandler.SaveData.Characters[Guid.ToHexString()].InventoryList;

            var itemsSO = itemsGuid.Select((ItemSaveHandler.GetItem)).ToList();
            itemsSO.RemoveAll((item => item == null));

            InventoryList = itemsSO;
        }

        public bool IsItemEquiped(ArmorItemSO armorItemSo)
        {
            Load();

            if (armorItemSo == ChestArmor || armorItemSo == HelmArmor || armorItemSo == GauntletsArmor ||
                armorItemSo == LeggingArmor) return true;
            else return false;
        }
        
        public bool IsItemInInventory(BaseItemSO baseItemTemplateSo)
        {
            Load();
            return InventoryList.Contains(baseItemTemplateSo);
        }

        public void EquipItem(ArmorItemSO itemSO)
        {
            if (IsItemEquiped(itemSO))
            {
                return;
            }

            switch (itemSO.ArmorType)
            {
                case ArmorType.Helm:
                    m_HelmArmor = itemSO;
                    break;
                case ArmorType.Chest:
                    m_ChestArmor = itemSO;
                    break;
                case ArmorType.Gauntlets:
                    m_GauntletsArmor = itemSO;
                    break;
                case ArmorType.Legging:
                    m_LeggingArmor = itemSO;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            Save();
        }

        public void UnequipItem(ArmorItemSO itemTemplate)
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

        public void AddInventory(BaseItemSO itemSO)
        {
            if (IsItemInInventory(itemSO))
            {
                return;
            }
            
            InventoryList.Add(itemSO);
            Save();
        }
        
        public void RemoveInventory(BaseItemSO itemSO)
        {
            if (!IsItemInInventory(itemSO))
            {
                return;
            }
            
            if (IsItemEquiped(itemSO as ArmorItemSO))
            {
                UnequipItem(itemSO as ArmorItemSO);
            }
            
            InventoryList.Remove(itemSO);
            
            Save();
        }

        public void ClearEquipment()
        {
            m_HelmArmor = null;
            m_ChestArmor = null;
            m_GauntletsArmor = null;
            m_LeggingArmor = null;
        }
        
        public bool HasEnoughCurrency(int amount)
        {
            Load();
            return Currency >= amount;
        }
        
        public void SpendCurrency(int amount)
        {
            Load();
            m_Currency -= amount;

            if (m_Currency < 0)
            {
                m_Currency = 0;
                Debug.Log("Currency cant be less than zero!!!!");
            }
            Save();
        }
        
          
        public void GainCurrency(int amount)
        {
            Load();
            m_Currency += amount;
            Save();
        }
    }
    
   
}