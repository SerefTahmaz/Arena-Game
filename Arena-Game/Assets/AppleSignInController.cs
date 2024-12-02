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
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class AppleSignInController : MonoBehaviour
{
    [SerializeField] private TMP_Text m_Email;
    [SerializeField] private TMP_Text m_UserName;
    [SerializeField] private TMP_Text m_UserId;
    
    private IAppleAuthManager appleAuthManager;

    void Start()
    {
        // If the current platform is supported
        if (AppleAuthManager.IsCurrentPlatformSupported)
        {
            // Creates a default JSON deserializer, to transform JSON Native responses to C# instances
            var deserializer = new PayloadDeserializer();
            // Creates an Apple Authentication manager with the deserializer
            this.appleAuthManager = new AppleAuthManager(deserializer);    
        }
    }

    void Update()
    {
        // Updates the AppleAuthManager instance to execute
        // pending callbacks inside Unity's execution loop
        if (this.appleAuthManager != null)
        {
            this.appleAuthManager.Update();
        }
    }

    private void PerformFirebaseAuthentication(
        IAppleIDCredential appleIdCredential,
        string rawNonce,
        Action<FirebaseUser> firebaseAuthCallback)
    {
        var identityToken = Encoding.UTF8.GetString(appleIdCredential.IdentityToken);
        var authorizationCode = Encoding.UTF8.GetString(appleIdCredential.AuthorizationCode);
        var firebaseCredential = OAuthProvider.GetCredential(
            "apple.com",
            identityToken,
            rawNonce,
            authorizationCode);

        FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(firebaseCredential)
            .ContinueWithOnMainThread(task => HandleSignInWithUser(task, firebaseAuthCallback));
    }

    private void HandleSignInWithUser(Task<FirebaseUser> task, Action<FirebaseUser> firebaseUserCallback)
    {
        if (task.IsCanceled)
        {
            Debug.Log("Firebase auth was canceled");
            firebaseUserCallback(null);
        }
        else if (task.IsFaulted)
        {
            Debug.Log("Firebase auth failed");
            firebaseUserCallback(null);
        }
        else
        {
            var firebaseUser = task.Result;
            Debug.Log("Firebase auth completed | User ID:" + firebaseUser.UserId);
            m_UserName.text = firebaseUser.DisplayName;
            m_Email.text = firebaseUser.Email;
            m_UserId.text = firebaseUser.UserId;
            
            firebaseUserCallback(firebaseUser);
        }
    }
    
    public void PerformQuickLoginWithFirebase(Action<FirebaseUser> firebaseAuthCallback)
    {
        var rawNonce = GenerateRandomString(32);
        var nonce = GenerateSHA256NonceFromRawNonce(rawNonce);

        var quickLoginArgs = new AppleAuthQuickLoginArgs(nonce);

        this.appleAuthManager.QuickLogin(
            quickLoginArgs,
            credential =>
            {
                var appleIdCredential = credential as IAppleIDCredential;
                if (appleIdCredential != null)
                {
                    this.PerformFirebaseAuthentication(appleIdCredential, rawNonce, firebaseAuthCallback);
                }
            },
            error =>
            {
                // Something went wrong
            });
    }

    public void PerformLoginWithAppleIdAndFirebase()
    {
        PerformLoginWithAppleIdAndFirebase((user =>{} ));
    }
    
    public void PerformLoginWithAppleIdAndFirebase(Action<FirebaseUser> firebaseAuthCallback)
    {
        var rawNonce = GenerateRandomString(32);
        var nonce = GenerateSHA256NonceFromRawNonce(rawNonce);

        var loginArgs = new AppleAuthLoginArgs(
            LoginOptions.IncludeEmail | LoginOptions.IncludeFullName,
            nonce);

        this.appleAuthManager.LoginWithAppleId(
            loginArgs,
            credential =>
            {
                var appleIdCredential = credential as IAppleIDCredential;
                if (appleIdCredential != null)
                {
                    this.PerformFirebaseAuthentication(appleIdCredential, rawNonce, firebaseAuthCallback);
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