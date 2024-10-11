using ArenaGame.Managers.SaveManager;
using DefaultNamespace;
using UnityEngine;

namespace Gameplay.Farming
{
    [CreateAssetMenu(fileName = "Tomato Item", menuName = "Game/Item/Seed Item", order = 0)]
    public class SeedItemSO : BaseItemSO, ISellableItem
    {
        [SerializeField] private SeedItemTemplateSO m_SeedItemTemplate;

        public int Price => m_SeedItemTemplate.Price;

        public SeedItemTemplateSO SeedItemTemplate
        {
            get => m_SeedItemTemplate;
            set => m_SeedItemTemplate = value;
        }

        public override Sprite ItemSprite => SeedItemTemplate.ItemSprite;

        public override void Save()
        {
            base.Save();
            ItemSaveHandler.Load();
            var savedItemData = ItemSaveHandler.GetSavedItemData<SeedItem>(Guid.ToHexString());
            
            if (savedItemData == null)
            {
                ItemSaveHandler.SaveData.SeedItems.Add(Guid.ToHexString(), new SeedItem());
                savedItemData = ItemSaveHandler.GetSavedItemData<SeedItem>(Guid.ToHexString());
                
                Debug.Log($"Is null {ItemSaveHandler.GetSavedItemData<SeedItem>(Guid.ToHexString()) == null}");
            }
        
            savedItemData.m_ItemName = m_ItemName;
            savedItemData.m_ItemType = m_ItemType;

            savedItemData.m_SeedItemTemplateGUID = SeedItemTemplate ? SeedItemTemplate.Guid.ToHexString() : "";
            ItemSaveHandler.SaveData.SeedItems[Guid.ToHexString()] = savedItemData;
            
            ItemSaveHandler.Save();
            OnChanged?.Invoke();
        }

        public override void Load()
        {
            base.Load();
            ItemSaveHandler.Load();
            var savedItemData = ItemSaveHandler.GetSavedItemData<SeedItem>(Guid.ToHexString());
            
            if (savedItemData != null)
            {
                m_ItemName = savedItemData.m_ItemName;
                m_ItemType = savedItemData.m_ItemType;

                var templateItemGuid = savedItemData.m_SeedItemTemplateGUID;
                SeedItemTemplate = ItemListSO.GetItemByGuid<SeedItemTemplateSO>(templateItemGuid);
            }
        }

        public PlantItemSO GivePlantItemInsSO()
        {
            var insPlantToBeBorn = SeedItemTemplate.PlantToBeBorn.DuplicateUnique();
            return insPlantToBeBorn;
        }
    }
}