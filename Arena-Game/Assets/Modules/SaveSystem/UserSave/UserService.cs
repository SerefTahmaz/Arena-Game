﻿using System;
using ArenaGame.Managers.SaveManager;
using Cysharp.Threading.Tasks;
using Firebase.Database;
using Firebase.Storage;
using Newtonsoft.Json;
using UnityEngine;

namespace Authentication
{
    public static class UserService
    {
        public static async UniTask<UserData> FetchUser(string uid)
        {
            var snapshot = await FirebaseRef.REF_USERS.Child(uid).GetValueAsync();
            if (!snapshot.Exists) return null;
            var user =  JsonConvert.DeserializeObject<UserData>(snapshot.GetRawJsonValue());
            return user;
        }
        
        public static async UniTask UpdateUser(string uid, UserData userData)
        {
            var serializedUser = JsonConvert.SerializeObject(userData);
            await FirebaseRef.REF_USERS.Child(uid).SetRawJsonValueAsync(serializedUser).AsUniTask();
        }
        
        public static async UniTask<RequestResult> UpdateProfileImage(Texture2D profileImage)
        {
            var imageData = profileImage.EncodeToJPG(30);
            var fileName = Guid.NewGuid().ToString();
            var storageRef = FirebaseRef.STORAGE_PROFILE_IMAGES.Child(fileName);

            var task =  storageRef.PutBytesAsync(imageData);
            await task;
            
            if (task.IsFaulted || task.IsCanceled) {
                Debug.Log(task.Exception.ToString());
                // Uh-oh, an error occurred!
                return RequestResult.Failed;
            }
            
            // Metadata contains file metadata such as size, content-type, and download URL.
            StorageMetadata metadata = task.Result;
            string md5Hash = metadata.Md5Hash;
            Debug.Log("Finished uploading...");
            Debug.Log("md5 hash = " + md5Hash);

            var profileImageUrlTask = metadata.Reference.GetDownloadUrlAsync();

            await profileImageUrlTask;
            if (profileImageUrlTask.IsFaulted || profileImageUrlTask.IsCanceled)
            {
                return RequestResult.Failed;
            }
                
            Debug.Log("Download URL: " + profileImageUrlTask.Result);
            // ... now download the file via WWW or UnityWebRequest.

            var profileImageUrl = profileImageUrlTask.Result.OriginalString;
            UserSaveHandler.SaveData.m_ProfileImageUrl = profileImageUrl;
            await UserSaveHandler.Save();
            return RequestResult.Success;
        }
    }
}