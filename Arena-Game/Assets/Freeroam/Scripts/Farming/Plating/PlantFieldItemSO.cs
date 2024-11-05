using System;
using System.Collections.Generic;
using System.Linq;
using ArenaGame.Managers.SaveManager;
using Cysharp.Threading.Tasks;
using Gameplay.Item;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "Plant Field Item", menuName = "Game/Item/Plant Field Item", order = 0)]
    public class PlantFieldItemSO : BaseItemSO
    {
        [SerializeField] private List<PlantItemSO> m_PlantList;

        public Action OnChanged { get; set; }

        public List<PlantItemSO> PlantList
        {
            get => m_PlantList;
            set => m_PlantList = value;
        }

        public override Sprite ItemSprite => null;

        public override void Save()
        {
            ItemSaveHandler.Load();
            if (!ItemSaveHandler.SaveData.PlantFieldItems.ContainsKey(Guid.ToHexString()))
            {
                ItemSaveHandler.SaveData.PlantFieldItems.Add(Guid.ToHexString(),new PlantFieldItem());
            }
            
            ItemSaveHandler.SaveData.PlantFieldItems[Guid.ToHexString()].m_PlantItems = PlantList.Select((item =>
            {
                string guid = "empty";
                if (item != null)
                {
                    item.Save();
                    guid = item.Guid.ToHexString();;
                }
                return guid;
            })).ToList();
            
            ItemSaveHandler.Save();
            OnChanged?.Invoke();
        }

        public override void Load()
        {
            ItemSaveHandler.Load();
            if (ItemSaveHandler.SaveData.PlantFieldItems.ContainsKey(Guid.ToHexString()))
            {
                //Convert to items
                LoadPlantFieldItems();
            }
        }
        
        private void LoadPlantFieldItems()
        {
            var itemsGuid = ItemSaveHandler.SaveData.PlantFieldItems[Guid.ToHexString()].m_PlantItems;
            var itemsSO = itemsGuid.Select((s => ItemSaveHandler.GetItem(s) as PlantItemSO)).ToList();
            // itemsSO.RemoveAll((item => item == null));
            PlantList = itemsSO;
        }
    }
}