using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DemoBlast.Managers.SaveManager
{
    public static class cSaveDataHandler
    {
        private static cSaveData m_SaveData = new cSaveData();
        public static cSaveData SaveData
        {
            get => m_SaveData;
            set => m_SaveData = value;
        }

        private static bool m_Loaded = false;

        private static string m_SaveFilePath => Application.persistentDataPath + "/SavaData.json";

        public static void Load(){
            if(m_Loaded) return;
        
            if (File.Exists(m_SaveFilePath))
            {
                string loadPlayerData = File.ReadAllText(m_SaveFilePath);
                SaveData = JsonUtility.FromJson<cSaveData>(loadPlayerData);
  
                // Debug.Log("Load game complete!");
                m_Loaded = true;
            }
            // else
            //     Debug.Log("There is no save files to load!");
        }


        public static void Save()
        {
            string savePlayerData = JsonUtility.ToJson(SaveData);
            File.WriteAllText(m_SaveFilePath, savePlayerData);

            // Debug.Log("Save file created at: ");
        }

#if UNITY_EDITOR
        [MenuItem("SaveData/DeleteSaveData")]
        public static void DeleteSaveFile()
        {
            if (File.Exists(m_SaveFilePath))
            {
                File.Delete(m_SaveFilePath);
                SaveData = new cSaveData();
  
                Debug.Log("Save file deleted!");
            }
            else
                Debug.Log("There is nothing to delete!");

            m_Loaded = false;
        }
#endif
       
    }
}