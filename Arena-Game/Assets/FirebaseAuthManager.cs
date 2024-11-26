using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using UnityEngine;

public class FirebaseAuthManager : MonoBehaviour,IAuthService
{
    private FirebaseAuth auth;
    private FirebaseUser user;

    public void Init()
    {
        InitializeFirebase();
    }

    void InitializeFirebase() {
            auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            auth.StateChanged += AuthStateChanged;
            AuthStateChanged(this, null);
        }

        void AuthStateChanged(object sender, System.EventArgs eventArgs) {
            if (auth.CurrentUser != user) {
                bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null
                                                         && auth.CurrentUser.IsValid();
                if (!signedIn && user != null) {
                    Debug.Log("Signed out " + user.UserId);
                }
                user = auth.CurrentUser;
                if (signedIn) {
                    Debug.Log("Signed in " + user.UserId);
                    var displayName = user.DisplayName ?? "";
                    var emailAddress = user.Email ?? "";
                    var photoUrl = user.PhotoUrl ?? default;
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

        public async UniTask<RequestResult> CreateUserWithMailAndPassword(string mail, string password)
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
}

public interface IAuthService
{
    UniTask<RequestResult> SignInWithMailAndPassword(string mail, string password);
    UniTask<RequestResult> CreateUserWithMailAndPassword(string mail, string password);
}

public enum RequestResult
{
    Failed,
    Success
}