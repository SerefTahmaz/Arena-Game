using System.Collections.Generic;
using System.IO;
using System.Linq;
using Authentication;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Gameplay.Farming;
using Mono.CSharp;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace ArenaGame.Managers.SaveManager
{
    public static class ItemSaveHandler
    {
        private static ItemData m_SaveData = new ItemData();
        public static ItemData SaveData
        {
            get => m_SaveData;
            set => m_SaveData = value;
        }

        public static bool m_Loaded = false;

        private static string m_SaveFilePath => Application.persistentDataPath  + "/ItemData-"+AuthManager.Instance.Uid+".json";

        public static async UniTask Load(){
            if(m_Loaded) return;
        
            m_GeneratedArmorItems.Clear();
            m_GeneratedFoodItems.Clear();
            m_GeneratedPlantItems.Clear();
            m_GeneratedSeedItems.Clear();
            m_GeneratedPlantFieldItems.Clear();
            
            // if (File.Exists(m_SaveFilePath))
            // {
            //     string loadPlayerData = File.ReadAllText(m_SaveFilePath);
            //     SaveData = JsonConvert.DeserializeObject<ItemData>(loadPlayerData);
            //
            //     // Debug.Log("Load game complete!");
            //     m_Loaded = true;
            // }
            
            if (AuthManager.Instance.IsAuthenticated)
            {
                var itemData = await ItemService.FetchItems(AuthManager.Instance.Uid);
                SaveData = itemData == null ? new ItemData() : itemData;
                m_Loaded = true;
            }
        }

        public static void Save()
        {
            if (!m_Loaded)
            {
                Load();
            }
            
            // string savePlayerData = JsonConvert.SerializeObject(SaveData);
            // File.WriteAllText(m_SaveFilePath, savePlayerData);
            
            if (AuthManager.Instance.IsAuthenticated)
            {
                ItemService.UpdateItems(AuthManager.Instance.Uid,SaveData);
            }
        }

        public static BaseItemSO GetItem(string guid)
        { 
            var insArmor = GetArmorItem(guid);
            if (insArmor != null) return insArmor;
            var insWeapon = GetWeaponItem(guid);
            if (insWeapon != null) return insWeapon;
            var insPlant = GetPlantItem(guid);
            if (insPlant != null) return insPlant;
            var insPlantField = GetPlantFieldItems(guid);
            if (insPlantField != null) return insPlantField;
            var insSeed = GetSeedItem(guid);
            if (insSeed != null) return insSeed;
            var insFood = GetFoodItem(guid);
            if (insFood != null) return insFood;
            
            Debug.Log($"Null item");
            
            return null;
        }

        public static BaseItemSO GetWeaponItem(string guid)
        {
            Load();
            if (!SaveData.WeaponItems.ContainsKey(guid)) return null;
            
            var ins = ScriptableObject.CreateInstance<ArmorItemSO>();
            ins.SetGuid(guid);
            ins.Load();
            return ins;
        }

        private static Dictionary<string, ArmorItemSO> m_GeneratedArmorItems = new Dictionary<string, ArmorItemSO>();

        public static ArmorItemSO GetArmorItem(string guid)
        {
            Load();
            if (!SaveData.ArmorItems.ContainsKey(guid)) return null;
            return GetItem<ArmorItemSO>(guid,m_GeneratedArmorItems);
        }
        
        private static Dictionary<string, PlantItemSO> m_GeneratedPlantItems = new Dictionary<string, PlantItemSO>();

        public static PlantItemSO GetPlantItem(string guid)
        {
            Load();
            if (!SaveData.PlantItems.ContainsKey(guid)) return null;
            return GetItem<PlantItemSO>(guid,m_GeneratedPlantItems);
        }
        
        private static Dictionary<string, PlantFieldItemSO> m_GeneratedPlantFieldItems = new Dictionary<string, PlantFieldItemSO>();

        public static PlantFieldItemSO GetPlantFieldItems(string guid)
        {
            Load();
            if (!SaveData.PlantItems.ContainsKey(guid)) return null;
            return GetItem<PlantFieldItemSO>(guid,m_GeneratedPlantFieldItems);
        }
        
        private static Dictionary<string, SeedItemSO> m_GeneratedSeedItems = new Dictionary<string, SeedItemSO>();

        public static SeedItemSO GetSeedItem(string guid)
        {
            Load();
            if (!SaveData.SeedItems.ContainsKey(guid)) return null;
            return GetItem<SeedItemSO>(guid,m_GeneratedSeedItems);
        }
        
        private static Dictionary<string, FoodItemSO> m_GeneratedFoodItems = new Dictionary<string, FoodItemSO>();

        public static FoodItemSO GetFoodItem(string guid)
        {
            Load();
            if (!SaveData.FoodItems.ContainsKey(guid)) return null;
            return GetItem<FoodItemSO>(guid,m_GeneratedFoodItems);
        }

        private static T GetItem<T>(string guid,  Dictionary<string, T> cacheDataBase) where T : BaseItemSO
        {
            if (cacheDataBase.ContainsKey(guid)) return cacheDataBase[guid];
            
            var ins = ScriptableObject.CreateInstance<T>();
            ins.SetGuid(guid);
            ins.Load();
            
            cacheDataBase.Add(guid, ins);
            return ins;
        }

        public static T GetSavedItemData<T>(string guid) where T : class
        {
            Load();
            if (SaveData.ArmorItems.ContainsKey(guid) && SaveData.ArmorItems[guid] is T foundArmorItem)
            {
                return foundArmorItem;
            }
            if (SaveData.PlantItems.ContainsKey(guid) && SaveData.PlantItems[guid] is T foundPlantItem)
            {
                return foundPlantItem;
            }
            if (SaveData.PlantFieldItems.ContainsKey(guid) && SaveData.PlantFieldItems[guid] is T foundPlantFieldItem)
            {
                return foundPlantFieldItem;
            }
            if (SaveData.SeedItems.ContainsKey(guid) && SaveData.SeedItems[guid] is T foundSeedItem)
            {
                return foundSeedItem;
            }
            if (SaveData.FoodItems.ContainsKey(guid) && SaveData.FoodItems[guid] is T foundFoodItem)
            {
                return foundFoodItem;
            }

            return null;
        }

#if UNITY_EDITOR
        [MenuItem("SaveData/Item/DeleteSaveData")]
        public static void DeleteSaveFile()
        {
            if (File.Exists(m_SaveFilePath))
            {
                File.Delete(m_SaveFilePath);
                SaveData = new ItemData();
  
                Debug.Log("Save file deleted!");
            }
            else
                Debug.Log("There is nothing to delete!");

            m_Loaded = false;
        }
        
        [MenuItem("SaveData/Item/ShowFileLoc")]
        public static void ShowFileLoc()
        {
            if (File.Exists(Application.persistentDataPath))
            {
                EditorUtility.RevealInFinder( Application.persistentDataPath);
            }
            else
                Debug.Log("There is nothing to show!");
        }
#endif
       
    }
}