using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Storage;
using UnityEngine;

public static class FirebaseRef
{
    public static StorageReference STORAGE_REF = FirebaseStorage.DefaultInstance.RootReference;
    public static StorageReference STORAGE_PROFILE_IMAGES = STORAGE_REF.Child("profile_images");
    public static DatabaseReference DB_REF = FirebaseDatabase.DefaultInstance.RootReference;
    public static DatabaseReference REF_USERS = DB_REF.Child("users");
    public static DatabaseReference REF_CHARACTERS = DB_REF.Child("characters");
}

public class FirebaseAuthManager : MonoBehaviour,IAuthService
{
    private FirebaseAuth auth;
    private FirebaseUser user;

    public FirebaseUser User => user;

    public async UniTask Init()
    {
        await InitializeFirebase();
    } 

    public async UniTask InitializeFirebase()
    {
        bool isInitCompleted = false;
        await Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                var app = Firebase.FirebaseApp.DefaultInstance;
                auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                auth.StateChanged += AuthStateChanged;
                AuthStateChanged(this, null);
                
                //TODO: ENABLE ON DEVICE
                FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            } else {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }

            isInitCompleted = true;
        });

        await UniTask.WaitUntil((() => isInitCompleted));
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs) {
        if (auth.CurrentUser != User) {
            bool signedIn = User != auth.CurrentUser && auth.CurrentUser != null
                                                     && auth.CurrentUser.IsValid();
            if (!signedIn && User != null) {
                Debug.Log("Signed out " + User.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn) {
                Debug.Log("Signed in " + User.UserId);
                var displayName = User.DisplayName ?? "";
                var emailAddress = User.Email ?? "";
                var photoUrl = User.PhotoUrl ?? default;
            }
        }
    }

    void OnDestroy() {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    public async UniTask<RequestResult> SignInWithMailAndPassword(string mail, string password)
    {
        RequestResult requestResult = RequestResult.Failed;
            
        await auth.SignInWithEmailAndPasswordAsync(mail, password).ContinueWith((task =>
        {
            if (task.IsCanceled) { 
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                requestResult = RequestResult.Failed;
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                requestResult = RequestResult.Failed;
                return;
            }

            AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
            requestResult = RequestResult.Success;
        }));

            
        return requestResult;
    }

    public async UniTask<RequestResult> RegisterUser(AuthCredentials authCredentials)
    {
        var imageData = authCredentials.ProfileImage.EncodeToJPG(30);
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

        var authTask= auth.CreateUserWithEmailAndPasswordAsync(authCredentials.Email, authCredentials.Password);
        await authTask;
                    
        if (authTask.IsCanceled) {
            Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
            return RequestResult.Failed;
        }
        if (authTask.IsFaulted) {
            Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
            return RequestResult.Failed;
        }

        // Firebase user has been created.
        AuthResult result = authTask.Result;
        Debug.LogFormat("Firebase user created successfully: {0} ({1})",
            result.User.DisplayName, result.User.UserId);

        var uid = result.User.UserId;

        var values = new Dictionary<string, object>()
        {
            { "m_Email", authCredentials.Email },
            { "m_Username", authCredentials.Username },
            { "m_ProfileImageUrl", profileImageUrl }
        };

        var databaseTask = FirebaseRef.REF_USERS.Child(uid).UpdateChildrenAsync(values);
        await databaseTask;

        if (databaseTask.IsFaulted || databaseTask.IsCanceled)
        {
            return RequestResult.Failed;
        }

        return RequestResult.Success;
    }

    private async UniTask<RequestResult> CreateUserWithMailAndPassword(string mail, string password)
    {
        RequestResult requestResult = RequestResult.Failed;
            
        await auth.CreateUserWithEmailAndPasswordAsync(mail, password).ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                requestResult = RequestResult.Failed;
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                requestResult = RequestResult.Failed;
                return;
            }

            // Firebase user has been created.
            AuthResult result = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
            requestResult = RequestResult.Success;
        });
            
        return requestResult;
    }

    public void SignOut()
    {
        Debug.Log("Sign out call");
        FirebaseAuth.DefaultInstance.SignOut();
    }
}

public interface IAuthService
{
    UniTask<RequestResult> SignInWithMailAndPassword(string mail, string password);
    // UniTask<RequestResult> CreateUserWithMailAndPassword(string mail, string password);
    UniTask<RequestResult> RegisterUser(AuthCredentials authCredentials);
    void SignOut();
}

public struct AuthCredentials
{
    public AuthCredentials(string email, string password, string username, Texture2D profileImage)
    {
        Email = email;
        Password = password;
        Username = username;
        ProfileImage = profileImage;
    }

    public string Email { get; }
    public string Password { get; }
    public string Username { get; }
    public Texture2D ProfileImage { get; }
}

public enum RequestResult
{
    Failed,
    Success
}