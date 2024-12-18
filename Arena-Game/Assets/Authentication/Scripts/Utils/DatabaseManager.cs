using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Managers.SaveManager;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DatabaseManager : MonoBehaviour
{
    // private void Start()
    // {
    //     FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(true);
    // }
    //
    // public void Save()
    // { 
    //     WriteAndReadSave();
    // }
    //
    // private async UniTask WriteAndReadSave()
    // {
    //     SaveGameHandler.Load();
    //     var saveData = SaveGameHandler.SaveData;
    //     var saveDataJson = JsonConvert.SerializeObject(saveData);
    //     
    //     var firebaseDatabase = FirebaseDatabase.DefaultInstance.RootReference;
    //     await firebaseDatabase.Child("users").Child(SystemInfo.deviceUniqueIdentifier).SetRawJsonValueAsync(saveDataJson).AsUniTask();
    //     var readData = await firebaseDatabase.Child("users").Child(SystemInfo.deviceUniqueIdentifier).GetValueAsync().AsUniTask();
    //     var deserializedData = JsonConvert.DeserializeObject<SaveData>(readData.GetRawJsonValue());
    //     Debug.Log($"{deserializedData.m_PlayerName} {deserializedData.m_ExperiencePoint}");
    // }
    //
    // private void writeNewUser(string userId, string name, string email) {
    //     User user = new User(name, email);
    //     string json = JsonUtility.ToJson(user);
    //
    //     mDatabaseRef.Child("users").Child(userId).SetRawJsonValueAsync(json);
    // }
}


#if UNITY_EDITOR
[CustomEditor(typeof(DatabaseManager))]
public class DatabaseManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Click"))
        {
        }
    }
}
#endif
