using System;
using System.IO;
using Authentication;
using Cysharp.Threading.Tasks;
using Extensions.Unity.ImageLoader;
using Firebase.Database;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace ArenaGame.Managers.SaveManager
{
//     public static class CharacterSaveHandler
//     {
//         private static CharacterData m_SaveData = new CharacterData();
//         public static CharacterData SaveData
//         {
//             get => m_SaveData;
//             set => m_SaveData = value;
//         }
//
//         public static bool m_Loaded = false;
//
//         private static string m_SaveFilePath => Application.persistentDataPath + "/CharacterData.json";
//         
//         public static Action OnChanged = delegate { };
//         public static Action OnForceChanged = delegate { };
//
//         public static async UniTask Load(bool forceLoad=false){
//             if(m_Loaded&& !forceLoad) return;
//             //
//             // if (File.Exists(m_SaveFilePath))
//             // {
//             //     string loadPlayerData = File.ReadAllText(m_SaveFilePath);
//             //     SaveData = JsonConvert.DeserializeObject<CharacterData>(loadPlayerData);
//             //
//             //     // Debug.Log("Load game complete!");
//             //     m_Loaded = true;
//             // }
//             
//             if (AuthManager.Instance.IsAuthenticated)
//             {
//                 var characterData = await CharacterService.FetchCharacters(AuthManager.Instance.Uid);
//                 SaveData = characterData == null ? new CharacterData() : characterData;
//                 m_Loaded = true;
//                 OnChanged?.Invoke();
//                 if (forceLoad)
//                 {
//                     OnForceChanged?.Invoke();
//                 }
//             }
//             
//             // else
//             //     Debug.Log("There is no save files to load!");
//         }
//
//
//         public static void Save()
//         {
//             if (!m_Loaded)
//             {
//                 Load();
//             }
//             
//             // string savePlayerData = JsonConvert.SerializeObject(SaveData);
//             // File.WriteAllText(m_SaveFilePath, savePlayerData);
//
//             if (AuthManager.Instance.IsAuthenticated)
//             {
//                 CharacterService.UpdateCharacters(AuthManager.Instance.Uid,SaveData);
//
//                 // Debug.Log("Save file created at: ");
//                 OnChanged.Invoke();
//             }
//             
//             // Debug.Log("Save file created at: ");
//         }
//
//         public static DatabaseReference LoadData(string path)
//         {
//             return FirebaseRef.REF_CHARACTERS.Child(AuthManager.Instance.Uid).Child(path);
//         }
//
// #if UNITY_EDITOR
//         [MenuItem("SaveData/Character/DeleteSaveData")]
//         public static void DeleteSaveFile()
//         {
//             if (File.Exists(m_SaveFilePath))
//             {
//                 File.Delete(m_SaveFilePath);
//                 SaveData = new CharacterData();
//   
//                 Debug.Log("Save file deleted!");
//             }
//             else
//                 Debug.Log("There is nothing to delete!");
//
//             m_Loaded = false;
//         }
//         
//         [MenuItem("SaveData/Character/ShowFileLoc")]
//         public static void ShowFileLoc()
//         {
//             if (File.Exists(m_SaveFilePath))
//             {
//                 EditorUtility.RevealInFinder(m_SaveFilePath);
//             }
//             else
//                 Debug.Log("There is nothing to show!");
//         }
// #endif
//        
//     }
}