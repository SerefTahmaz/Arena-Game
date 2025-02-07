using ArenaGame.Managers.SaveManager;
using DefaultNamespace;
using UnityEngine;

namespace Gameplay.Farming
{
    [CreateAssetMenu(fileName = "Food Item", menuName = "Game/Item/Food Item", order = 0)]
    public class FoodItemSO : BaseItemSO, ISellableItem
    {
        [SerializeField] private FoodItemTemplateSO m_FoodItemTemplate;

        public int Price => m_FoodItemTemplate.Price;

        public FoodItemTemplateSO FoodItemTemplate
        {
            get => m_FoodItemTemplate;
            set => m_FoodItemTemplate = value;
        }

        public override Sprite ItemSprite => m_FoodItemTemplate.ItemSprite;

        public override void Save()
        {
            base.Save();
            ItemSaveHandler.Load();
            var savedItemData = ItemSaveHandler.GetSavedItemData<FoodItem>(Guid.ToHexString());
            
            if (savedItemData == null)
            {
                ItemSaveHandler.SaveData.FoodItems.Add(Guid.ToHexString(), new FoodItem());
                savedItemData = ItemSaveHandler.GetSavedItemData<FoodItem>(Guid.ToHexString());
                
                Debug.Log($"Is null {ItemSaveHandler.GetSavedItemData<FoodItem>(Guid.ToHexString()) == null}");
            }
        
            savedItemData.m_ItemName = m_ItemName;
            savedItemData.m_ItemType = m_ItemType;

            savedItemData.m_FoodItemTemplateGUID = FoodItemTemplate ? FoodItemTemplate.Guid.ToHexString() : "";
            ItemSaveHandler.SaveData.FoodItems[Guid.ToHexString()] = savedItemData;
            
            ItemSaveHandler.Save();
            OnChanged?.Invoke();
        }

        public override void Load()
        {
            base.Load();
            ItemSaveHandler.Load();
            var savedItemData = ItemSaveHandler.GetSavedItemData<FoodItem>(Guid.ToHexString());
            
            if (savedItemData != null)
            {
                m_ItemName = savedItemData.m_ItemName;
                m_ItemType = savedItemData.m_ItemType;

                var templateItemGuid = savedItemData.m_FoodItemTemplateGUID;
                FoodItemTemplate = ItemListSO.GetItemByGuid<FoodItemTemplateSO>(templateItemGuid);
            }
        }
    }
}