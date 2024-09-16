using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace ArenaGame.Managers.SaveManager
{
    public static class SaveGameHandler
    {
        private static SaveData m_SaveData = new SaveData();
        public static SaveData SaveData
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
                SaveData = JsonConvert.DeserializeObject<SaveData>(loadPlayerData);
  
                // Debug.Log("Load game complete!");
                m_Loaded = true;
            }
            // else
            //     Debug.Log("There is no save files to load!");
        }


        public static void Save()
        {
            string savePlayerData = JsonConvert.SerializeObject(SaveData);
            File.WriteAllText(m_SaveFilePath, savePlayerData);

            // Debug.Log("Save file created at: ");
        }

        public static Texture2D GetProfileImage()
        {
            if (File.Exists(Application.persistentDataPath + "/SavedProfileImages/SavedScreen.jpg"))
            {
                var rawImage = File.ReadAllBytes(Application.persistentDataPath + "/SavedProfileImages/SavedScreen.jpg");
                Texture2D readableText = new Texture2D(128, 128);
                if (ImageConversion.LoadImage(readableText, rawImage))
                {
                    return readableText;
                }
            }

            return null;
        }

        public static void SaveProfileImage(Texture2D texture)
        {
            byte[] bytes = ImageConversion.EncodeToJPG(DuplicateTexture(texture));

            if (!Directory.Exists(Application.persistentDataPath + "/SavedProfileImages"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/SavedProfileImages");
            }

            // Write the returned byte array to a file in the project folder
            File.WriteAllBytes(Application.persistentDataPath + "/SavedProfileImages/SavedScreen.jpg", bytes);

#if UNITY_EDITOR
            EditorUtility.RevealInFinder(Application.persistentDataPath);
#endif
        }
        
        private static Texture2D DuplicateTexture(Texture2D source)
        {
            var width = 128;
            var height = 128;
        
            RenderTexture renderTex = RenderTexture.GetTemporary(
                width,
                height,
                0,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(width, height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }

#if UNITY_EDITOR
        [MenuItem("SaveData/DeleteSaveData")]
        public static void DeleteSaveFile()
        {
            if (File.Exists(m_SaveFilePath))
            {
                File.Delete(m_SaveFilePath);
                SaveData = new SaveData();
  
                Debug.Log("Save file deleted!");
            }
            else
                Debug.Log("There is nothing to delete!");

            m_Loaded = false;
        }
#endif
       
    }
}