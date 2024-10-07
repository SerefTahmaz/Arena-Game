using System;
using ArenaGame.Managers.SaveManager;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "Armor Item", menuName = "Game/Item/Plant Item", order = 0)]
    public class PlantItemSO : BaseItemSO
    {
        [SerializeField] private PlantState m_PlantState;
        [SerializeField] private PlantItemTemplate m_PlantItemTemplate;
        private DateTime m_CreationDate;

        public PlantState PlantState
        {
            get => m_PlantState;
            set => m_PlantState = value;
        }

        public PlantItemTemplate PlantItemTemplate
        {
            get => m_PlantItemTemplate;
            set => m_PlantItemTemplate = value;
        }

        public DateTime CreationDate
        {
            get => m_CreationDate;
            set => m_CreationDate = value;
        }

        public override Sprite ItemSprite => m_PlantItemTemplate.ItemSprite;

        public override void Save()
        {
            base.Save();
            ItemSaveHandler.Load();
            if (!ItemSaveHandler.SaveData.PlantItems.ContainsKey(Guid.ToHexString()))
            {
                ItemSaveHandler.SaveData.PlantItems.Add(Guid.ToHexString(), new PlantItem());
            }
            
            ItemSaveHandler.SaveData.PlantItems[Guid.ToHexString()].m_PlantState = PlantState;
            ItemSaveHandler.SaveData.PlantItems[Guid.ToHexString()].m_CreationDate = CreationDate;
        
            ItemSaveHandler.SaveData.PlantItems[Guid.ToHexString()].m_ItemName = m_ItemName;
            ItemSaveHandler.SaveData.PlantItems[Guid.ToHexString()].m_ItemType = m_ItemType;

            ItemSaveHandler.SaveData.PlantItems[Guid.ToHexString()].m_PlantItemTemplate = PlantItemTemplate ? PlantItemTemplate.Guid.ToHexString() : "";
            
            ItemSaveHandler.Save();
            OnChanged?.Invoke();
        }

        public override void Load()
        {
            base.Load();
            ItemSaveHandler.Load();
            if (ItemSaveHandler.SaveData.PlantItems.ContainsKey(Guid.ToHexString()))
            {
                PlantState = ItemSaveHandler.SaveData.PlantItems[Guid.ToHexString()].m_PlantState;
                CreationDate = ItemSaveHandler.SaveData.PlantItems[Guid.ToHexString()].m_CreationDate;

                m_ItemName = ItemSaveHandler.SaveData.PlantItems[Guid.ToHexString()].m_ItemName;
                m_ItemType = ItemSaveHandler.SaveData.PlantItems[Guid.ToHexString()].m_ItemType;

                var templateItemGuid =
                    ItemSaveHandler.SaveData.PlantItems[Guid.ToHexString()].m_PlantItemTemplate;
                PlantItemTemplate = ItemListSO.GetItemByGuid<PlantItemTemplate>(templateItemGuid);
            }
        }

        public void SetState(PlantState plantState)
        {
            Load();
            PlantState = plantState;
            Save();
        }
    }
    
    public enum PlantState
    {
        NewBorn,
        FullyGrown
    }
}