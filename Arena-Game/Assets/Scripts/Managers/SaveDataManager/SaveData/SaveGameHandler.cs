using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ArenaGame.Utils;
using Authentication;
using Cysharp.Threading.Tasks;
using Extensions.Unity.ImageLoader;
using Mono.CSharp;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace ArenaGame.Managers.SaveManager
{
    public static class SaveGameHandler
    {
        private static User m_SaveData = new User();
        public static User SaveData
        {
            get => m_SaveData;
            set => m_SaveData = value;
        }

        private static Texture2D m_ProfileImage;

        public static bool m_Loaded = false;

        private static string m_SaveFilePath => Application.persistentDataPath + "/SavaData.json";

        public static Action OnChanged = delegate { };

        public static async UniTask Load(bool forceLoad=false){
            if(m_Loaded&& !forceLoad) return;
        
            // if (File.Exists(m_SaveFilePath))
            // {
            //     string loadPlayerData = File.ReadAllText(m_SaveFilePath);
            //     SaveData = JsonConvert.DeserializeObject<User>(loadPlayerData);
            //
            //     // Debug.Log("Load game complete!");
            //     m_Loaded = true;
            // }
            // else
            //     Debug.Log("There is no save files to load!");
 
            if (AuthManager.Instance.IsAuthenticated)
            {
                var user = await UserService.FetchUser(AuthManager.Instance.Uid);
                SaveData = user;
                if (!string.IsNullOrEmpty(SaveData.m_ProfileImageUrl))
                {
                    var image = await ImageLoader.LoadSprite(SaveData.m_ProfileImageUrl).AsUniTask();
                    m_ProfileImage = image.texture;
                }
                m_Loaded = true;

                OnChanged?.Invoke();
            }
        }


        public static async UniTask Save()
        {
            if (!m_Loaded)
            {
                Load();
            }

            // string savePlayerData = JsonConvert.SerializeObject(SaveData);
            // File.WriteAllText(m_SaveFilePath, savePlayerData);

            if (AuthManager.Instance.IsAuthenticated)
            {
                UserService.UpdateUser(AuthManager.Instance.Uid,SaveData);

                // Debug.Log("Save file created at: ");
                OnChanged.Invoke();
            }
        }

        public static Texture2D GetProfileImage()
        {
            if (!m_Loaded)
            {
                Load();
            }
            // if (File.Exists(Application.persistentDataPath + "/SavedProfileImages/SavedScreen.jpg"))
            // {
            //     var rawImage = File.ReadAllBytes(Application.persistentDataPath + "/SavedProfileImages/SavedScreen.jpg");
            //     Texture2D readableText = new Texture2D(128, 128);
            //     if (ImageConversion.LoadImage(readableText, rawImage))
            //     {
            //         return readableText;
            //     }
            // 

            return m_ProfileImage;
        }
 
        public static void SaveProfileImage(Texture2D texture)
        {
            // byte[] bytes = ImageConversion.EncodeToJPG(DuplicateTexture(texture));
            //
            // if (!Directory.Exists(Application.persistentDataPath + "/SavedProfileImages"))
            // {
            //     Directory.CreateDirectory(Application.persistentDataPath + "/SavedProfileImages");
            // }
            //
            // // Write the returned byte array to a file in the project folder
            // File.WriteAllBytes(Application.persistentDataPath + "/SavedProfileImages/SavedScreen.jpg", bytes);

            UserService.UpdateProfileImage(texture.DuplicateTexture());
            m_ProfileImage = texture;

            OnChanged?.Invoke();
// #if UNITY_EDITOR
//             EditorUtility.RevealInFinder(Application.persistentDataPath);
// #endif
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
                RenderTextureReadWrite.sRGB);

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
                SaveData = new User();
  
                Debug.Log("Save file deleted!");
            }
            else
                Debug.Log("There is nothing to delete!");

            m_Loaded = false;
        }
        
        [MenuItem("SaveData/ShowFileLoc")]
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