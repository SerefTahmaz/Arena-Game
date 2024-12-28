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
                    Debug.Log(" Something went wrong");
                    // Something went wrong
                });

            UniTask.WaitUntil((() => isAppleCallbackReceieved));
            if (appleCredential is IAppleIDCredential appleIdCredential)
            {
                await PerformFirebaseAuthentication(appleIdCredential, rawNonce);
            }
        }
        
        MiniLoadingScreen.Instance.HidePage(appleAuthLoadingToken);
        m_Button.Activate();
    }

    private async UniTask PerformFirebaseAuthentication(IAppleIDCredential appleIdCredential, string rawNonce)
    {
        var loadingToken = new object();
        MiniLoadingScreen.Instance.ShowPage(loadingToken);
        
        var identityToken = Encoding.UTF8.GetString(appleIdCredential.IdentityToken);
        var authorizationCode = Encoding.UTF8.GetString(appleIdCredential.AuthorizationCode);
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
    
    public async UniTask PerformQuickLoginWithFirebase()
    {
        var rawNonce = GenerateRandomString(32);
        var nonce = GenerateSHA256NonceFromRawNonce(rawNonce);

        var quickLoginArgs = new AppleAuthQuickLoginArgs(nonce);

        appleAuthManager.QuickLogin(
            quickLoginArgs,
            credential =>
            {
                var appleIdCredential = credential as IAppleIDCredential;
                if (appleIdCredential != null)
                {
                    PerformFirebaseAuthentication(appleIdCredential, rawNonce);
                }
            },
            error =>
            {
                // Something went wrong
            });
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