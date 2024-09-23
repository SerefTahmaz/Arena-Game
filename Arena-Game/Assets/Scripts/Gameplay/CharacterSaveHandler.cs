using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace ArenaGame.Managers.SaveManager
{
    public static class CharacterSaveHandler
    {
        private static CharacterData m_SaveData = new CharacterData();
        public static CharacterData SaveData
        {
            get => m_SaveData;
            set => m_SaveData = value;
        }

        private static bool m_Loaded = false;

        private static string m_SaveFilePath => Application.persistentDataPath + "/CharacterData.json";

        public static void Load(){
            if(m_Loaded) return;
        
            if (File.Exists(m_SaveFilePath))
            {
                string loadPlayerData = File.ReadAllText(m_SaveFilePath);
                SaveData = JsonConvert.DeserializeObject<CharacterData>(loadPlayerData);
  
                // Debug.Log("Load game complete!");
                m_Loaded = true;
            }
            // else
            //     Debug.Log("There is no save files to load!");
        }


        public static void Save()
        {
            if (!m_Loaded)
            {
                Load();
            }
            
            string savePlayerData = JsonConvert.SerializeObject(SaveData);
            File.WriteAllText(m_SaveFilePath, savePlayerData);

            // Debug.Log("Save file created at: ");
        }

#if UNITY_EDITOR
        [MenuItem("SaveData/Character/DeleteSaveData")]
        public static void DeleteSaveFile()
        {
            if (File.Exists(m_SaveFilePath))
            {
                File.Delete(m_SaveFilePath);
                SaveData = new CharacterData();
  
                Debug.Log("Save file deleted!");
            }
            else
                Debug.Log("There is nothing to delete!");

            m_Loaded = false;
        }
        
        [MenuItem("SaveData/Character/ShowFileLoc")]
        public static void ShowFileLoc()
        {
            if (File.Exists(m_SaveFilePath))
            {
                EditorUtility.RevealInFinder(m_SaveFilePath);
            }
            else
                Debug.Log("There is nothing to show!");
        }
#endif
       
    }
}