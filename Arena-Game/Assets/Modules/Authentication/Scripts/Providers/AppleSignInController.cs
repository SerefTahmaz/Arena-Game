using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Extensions;
using AppleAuth.Interfaces;
using AppleAuth.Native;
using Authentication;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class AppleSignInController : BaseAuthProvider
{
    [SerializeField] private cButton m_Button;
    private IAppleAuthManager appleAuthManager;

    private string m_identityTokenKey = "AppleidentityToken";
    private string m_authorizationCode = "AppleauthorizationCode";
    private string AppleUserIdKey = "AppleUserId";

    public override async UniTask Init(IAuthService authService)
    {
        await base.Init(authService);
        m_Button.OnClickEvent.AddListener(HandleButtonClicked);
    }

    void Start()
    {
        // If the current platform is supported
        if (AppleAuthManager.IsCurrentPlatformSupported)
        {
            // Creates a default JSON deserializer, to transform JSON Native responses to C# instances
            var deserializer = new PayloadDeserializer();
            // Creates an Apple Authentication manager with the deserializer
            appleAuthManager = new AppleAuthManager(deserializer);    
        }
    }

    void Update()
    {
        // Updates the AppleAuthManager instance to execute
        // pending callbacks inside Unity's execution loop
        if (appleAuthManager != null)
        {
            appleAuthManager.Update();
        }
    }

    public void HandleButtonClicked()
    {
        PerformLoginWithAppleIdAndFirebase();
    }
    
    public async UniTask PerformLoginWithAppleIdAndFirebase()
    {
        m_Button.DeActivate(); 
        var appleAuthLoadingToken = new object();
        MiniLoadingScreen.Instance.ShowPage(appleAuthLoadingToken);

        var appleId = PlayerPrefs.GetString(AppleUserIdKey);
        var isValidUser = await CheckCredentialStatusForUserId(appleId);
        
        if (isValidUser)
        {
            await AuthorizedLogIn();
        }
        else
        {
            var result = await PerformQuickLoginWithFirebase();
            if (!result)
            {
                await FirstTimeSignInWithFirebase(); 
            }
        }
        
        MiniLoadingScreen.Instance.HidePage(appleAuthLoadingToken);
        m_Button.Activate();
    }

    private async UniTask FirstTimeSignInWithFirebase()
    {
        var rawNonce = GenerateRandomString(32);
        var nonce = GenerateSHA256NonceFromRawNonce(rawNonce);

        var loginArgs = new AppleAuthLoginArgs(
            LoginOptions.IncludeEmail | LoginOptions.IncludeFullName,
            nonce);

        if (appleAuthManager != null)
        {
            bool isAppleCallbackReceieved = false;
            ICredential appleCredential = null;
            appleAuthManager.LoginWithAppleId(
                loginArgs,
                credential =>
                {
                    isAppleCallbackReceieved = true;
                    appleCredential = credential;
                },
                error =>
                {
                    isAppleCallbackReceieved = true;
                    Debug.Log("Apple SignIn: Something went wrong");
                    // Something went wrong
                });

            UniTask.WaitUntil((() => isAppleCallbackReceieved));
            if (appleCredential is IAppleIDCredential appleIdCredential)
            {
                await PerformFirebaseAuthenticationWithCredentials(appleIdCredential, rawNonce);
            }
            else
            {
                Debug.Log("appleCredential is not IAppleIDCredential!!!");
            }
        }
        else
        {
            Debug.Log("Apple auth manager is null!!!");
        }
    }
    
    private async UniTask<bool> CheckCredentialStatusForUserId(string appleUserId)
    {
        bool isAppleCallbackReceieved = false;
        bool result = false;
        // If there is an apple ID available, we should check the credential state
        appleAuthManager.GetCredentialState(
            appleUserId,
            state =>
            {
                switch (state)
                {
                    // If it's authorized, login with that user id
                    case CredentialState.Authorized:
                        result = true;
                        break;
                    
                    // If it was revoked, or not found, we need a new sign in with apple attempt
                    // Discard previous apple user id
                    case CredentialState.Revoked:
                    case CredentialState.NotFound:
                        result = false;
                        PlayerPrefs.DeleteKey(AppleUserIdKey);
                        break;
                }

                isAppleCallbackReceieved = true;
            },
            error =>
            {
                var authorizationErrorCode = error.GetAuthorizationErrorCode();
                Debug.LogWarning("Error while trying to get credential state " + authorizationErrorCode.ToString() + " " + error.ToString());
                result = false;
                isAppleCallbackReceieved = true;
            });
        
        await UniTask.WaitUntil((() => isAppleCallbackReceieved));
        return result;
    }

    private async UniTask AuthorizedLogIn()
    {
        var rawNonce = GenerateRandomString(32);
        var identityToken = PlayerPrefs.GetString(m_identityTokenKey);
        var authorizationCode = PlayerPrefs.GetString(m_authorizationCode);
        await PerformFirebaseAuthentication(identityToken, authorizationCode, rawNonce);
    }
    
    private async UniTask PerformFirebaseAuthenticationWithCredentials(IAppleIDCredential appleIdCredential, string rawNonce)
    {
        
        var identityToken = Encoding.UTF8.GetString(appleIdCredential.IdentityToken);
        var authorizationCode = Encoding.UTF8.GetString(appleIdCredential.AuthorizationCode);
        
        PlayerPrefs.SetString(AppleUserIdKey, appleIdCredential.User);
        PlayerPrefs.SetString(m_identityTokenKey, identityToken);    
        PlayerPrefs.SetString(m_authorizationCode, authorizationCode);
        
        await PerformFirebaseAuthentication(identityToken, authorizationCode, rawNonce);
    }
    
    private async UniTask PerformFirebaseAuthentication(string identityToken, string authorizationCode, string rawNonce)
    {
        var loadingToken = new object();
        MiniLoadingScreen.Instance.ShowPage(loadingToken);
        
        var firebaseCredential = OAuthProvider.GetCredential(
            "apple.com",
            identityToken,
            rawNonce,
            authorizationCode);

        var task = m_AuthService.SignInWithCredentialAsync(firebaseCredential);
        var user = await task;
        
        if (task.Status == UniTaskStatus.Canceled)
        {
            Debug.Log("Firebase auth was canceled");
        }
        else if (task.Status == UniTaskStatus.Faulted)
        {
            Debug.Log("Firebase auth failed");
        }
        else
        {
            Debug.Log("Firebase auth completed | User ID:" + user.UserId);
        }
        
        MiniLoadingScreen.Instance.HidePage(loadingToken);
    }

    // private async UniTask PerformFirebaseAuthentication(IAppleIDCredential appleIdCredential, string rawNonce)
    // {
    //     var loadingToken = new object();
    //     MiniLoadingScreen.Instance.ShowPage(loadingToken);
    //     
    //     var identityToken = Encoding.UTF8.GetString(appleIdCredential.IdentityToken);
    //     var authorizationCode = Encoding.UTF8.GetString(appleIdCredential.AuthorizationCode);
    //     var firebaseCredential = OAuthProvider.GetCredential(
    //         "apple.com",
    //         identityToken,
    //         rawNonce,
    //         authorizationCode);
    //
    //     var task = m_AuthService.SignInWithCredentialAsync(firebaseCredential);
    //     var user = await task;
    //     
    //     if (task.Status == UniTaskStatus.Canceled)
    //     {
    //         Debug.Log("Firebase auth was canceled");
    //     }
    //     else if (task.Status == UniTaskStatus.Faulted)
    //     {
    //         Debug.Log("Firebase auth failed");
    //     }
    //     else
    //     {
    //         Debug.Log("Firebase auth completed | User ID:" + user.UserId);
    //     }
    //     
    //     MiniLoadingScreen.Instance.HidePage(loadingToken);
    // }
    
    public async UniTask<bool> PerformQuickLoginWithFirebase()
    {
        var rawNonce = GenerateRandomString(32);
        var nonce = GenerateSHA256NonceFromRawNonce(rawNonce);

        var quickLoginArgs = new AppleAuthQuickLoginArgs(nonce);

        bool isAppleCallbackReceieved = false;
        bool signInResult = false;
        IAppleIDCredential credentials=null;
        appleAuthManager.QuickLogin(
            quickLoginArgs,
            credential =>
            {
                var appleIdCredential = credential as IAppleIDCredential;
                if (appleIdCredential != null)
                {
                    credentials = appleIdCredential;
                    signInResult = true;
                }
                else
                {
                    signInResult = false;
                }
                isAppleCallbackReceieved = true;
            },
            error =>
            {
                // Something went wrong
                signInResult = false;
                isAppleCallbackReceieved = true;
            });
        
        UniTask.WaitUntil((() => isAppleCallbackReceieved));
        if (signInResult&&credentials != null){ await PerformFirebaseAuthenticationWithCredentials(credentials, rawNonce);}
        return signInResult;
    }
    
    private static string GenerateSHA256NonceFromRawNonce(string rawNonce)
    {
        var sha = new SHA256Managed();
        var utf8RawNonce = Encoding.UTF8.GetBytes(rawNonce);
        var hash = sha.ComputeHash(utf8RawNonce);

        var result = string.Empty;
        for (var i = 0; i < hash.Length; i++)
        {
            result += hash[i].ToString("x2");
        }

        return result;
    }
    
    private static string GenerateRandomString(int length)
    {
        if (length <= 0)
        {
            throw new Exception("Expected nonce to have positive length");
        }

        const string charset = "0123456789ABCDEFGHIJKLMNOPQRSTUVXYZabcdefghijklmnopqrstuvwxyz-._";
        var cryptographicallySecureRandomNumberGenerator = new RNGCryptoServiceProvider();
        var result = string.Empty;
        var remainingLength = length;

        var randomNumberHolder = new byte[1];
        while (remainingLength > 0)
        {
            var randomNumbers = new List<int>(16);
            for (var randomNumberCount = 0; randomNumberCount < 16; randomNumberCount++)
            {
                cryptographicallySecureRandomNumberGenerator.GetBytes(randomNumberHolder);
                randomNumbers.Add(randomNumberHolder[0]);
            }

            for (var randomNumberIndex = 0; randomNumberIndex < randomNumbers.Count; randomNumberIndex++)
            {
                if (remainingLength == 0)
                {
                    break;
                }

                var randomNumber = randomNumbers[randomNumberIndex];
                if (randomNumber < charset.Length)
                {
                    result += charset[randomNumber];
                    remainingLength--;
                }
            }
        }

        return result;
    }
}