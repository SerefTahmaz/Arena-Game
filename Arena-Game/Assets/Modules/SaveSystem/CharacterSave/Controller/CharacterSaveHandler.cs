using System;
using System.Collections.Generic;
using System.Linq;
using ArenaGame.Managers.SaveManager;
using Authentication;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Firebase.Database;
using Gameplay;
using Gameplay.Item;
using Item;
using Newtonsoft.Json;
using UnityEngine;

namespace  ArenaGame.Managers.SaveManager
{
    public class CharacterSaveHandler
    {
        private int m_Health;
        private int m_Currency;
        private List<BaseItemSO> m_InventoryList = new List<BaseItemSO>();

        private ArmorItemSO m_HelmArmor;
        private ArmorItemSO m_ChestArmor;
        private ArmorItemSO m_GauntletsArmor;
        private ArmorItemSO m_LeggingArmor;

        public Action OnChanged { get; set; }
        public Action OnItemAddedInventory { get; set; }
        public Action OnItemRemovedInventory { get; set; }

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
        
        public string Guid { get; set; }
        public DatabaseReference DatabaseReference { get; set; }
        public CharacterSO CharacterSo { get; set; }

        public void Init(string guid, CharacterSO characterSo)
        {
            Guid = guid;
            DatabaseReference = CharacterService.FetchCharacter(AuthManager.Instance.Uid, Guid);
            CharacterSo = characterSo;
            // DatabaseReference.ValueChanged += DatabaseReferenceOnValueChanged;
        }
        //
        // private void DatabaseReferenceOnValueChanged(object sender, ValueChangedEventArgs e)
        // {
        //     var json = e.Snapshot.GetRawJsonValue();
        //     var characterData =  JsonConvert.DeserializeObject<Character>(json);
        //     Load(characterData);
        // }

        public async UniTask Save()
        {
            // CharacterSaveHandler.Load();
            var saveChar = new CharacterData();
            
            saveChar.Health = Health;
            saveChar.Currency = Currency;
            saveChar.InventoryList = InventoryList.Where((so => so!=null)).Select((item =>
            {
                item.Save();
                return item.Guid.ToHexString();
            })).ToList();

            saveChar.HelmArmor = HelmArmor != null ? HelmArmor.Guid.ToHexString() : "";
            saveChar.ChestArmor = ChestArmor != null ? ChestArmor.Guid.ToHexString() : "";
            saveChar.GaunletsArmor = GauntletsArmor != null ? GauntletsArmor.Guid.ToHexString() : "";
            saveChar.LeggingArmor = LeggingArmor != null ? LeggingArmor.Guid.ToHexString() : "";

            OnChanged?.Invoke();
            var serializedChar = JsonConvert.SerializeObject(saveChar);
            await DatabaseReference.SetRawJsonValueAsync(serializedChar);
        }

        public async UniTask Load()
        {
            var snapshot = await DatabaseReference.GetValueAsync();
            
            var json = snapshot.GetRawJsonValue();
            if (string.IsNullOrEmpty(json))
            {
                m_Health = CharacterSo.StartHealth;
                Save();
                return; 
            }
            var character =  JsonConvert.DeserializeObject<CharacterData>(json);
            
            m_Health = character.Health;
            m_Currency = character.Currency;
            //Convert to items

            LoadEquipmentList(character);
            LoadInventoryList(character);
            OnChanged?.Invoke();
        }


        private void LoadEquipmentList(CharacterData characterData)
        {
            m_HelmArmor = ItemSaveHandler.GetArmorItem(characterData.HelmArmor);
            m_ChestArmor = ItemSaveHandler.GetArmorItem(characterData.ChestArmor);
            m_GauntletsArmor = ItemSaveHandler.GetArmorItem(characterData.GaunletsArmor);
            m_LeggingArmor = ItemSaveHandler.GetArmorItem(characterData.LeggingArmor);
        }
        
        private void LoadInventoryList(CharacterData characterData)
        {
            var itemsGuid = characterData.InventoryList;

            var itemsSO = itemsGuid.Select((ItemSaveHandler.GetItem)).ToList();
            itemsSO.RemoveAll((item => item == null));

            InventoryList = itemsSO;
        }

        public bool IsItemEquiped(ArmorItemSO armorItemSo)
        {
            if (armorItemSo == ChestArmor || armorItemSo == HelmArmor || armorItemSo == GauntletsArmor ||
                armorItemSo == LeggingArmor) return true;
            else return false;
        }
        
        public bool IsItemInInventory(BaseItemSO baseItemTemplateSo)
        {
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
            OnItemAddedInventory?.Invoke();
        }
        
        public void RemoveInventory(BaseItemSO itemSO)
        {
            if (!IsItemInInventory(itemSO))
            {
                return;
            }
            
            if (itemSO is ArmorItemSO armorItemSo && IsItemEquiped(armorItemSo))
            {
                UnequipItem(armorItemSo);
            }
            
            InventoryList.Remove(itemSO);
            Save();
            OnItemRemovedInventory?.Invoke();
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
            return Currency >= amount;
        }
        
        public void SpendCurrency(int amount)
        {
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
            m_Currency += amount;
            Save();
        }
    }
    
   
}