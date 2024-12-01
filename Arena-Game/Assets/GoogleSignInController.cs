using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Extensions;
using Google;
using TMPro;
using UnityEngine;

public class GoogleSignInController : MonoBehaviour
{
    [SerializeField] private TMP_Text m_UserName;
    [SerializeField] private TMP_Text m_UserId;
    [SerializeField] private TMP_Text m_UserPP;
    
    private string api = "1068966136010-54pe02ev4aeeolbmc0prms92b3bnu46b.apps.googleusercontent.com";

    private GoogleSignInConfiguration m_Configuration;
    private FirebaseUser m_Fuser;

    private void Awake()
    {
        m_Configuration = new GoogleSignInConfiguration
        {
            WebClientId = api,
            RequestIdToken = true
        };
    }

    public void SignIn()
    {
        GoogleSignIn.Configuration = m_Configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;
        GoogleSignIn.DefaultInstance.SignIn().ContinueWithOnMainThread(FinishSignIn);
    }
    
    private void FinishSignIn(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted || task.IsCanceled)
        {
            Debug.Log("Kayıt hatası");
            return;
        }

        Credential credential = GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
        
        var auth = FirebaseAuth.DefaultInstance;
        auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread((signInTask =>
        {
            if (signInTask.IsFaulted || signInTask.IsCanceled)
            {
                Debug.Log("Credential hatası");
                return;
            } 

            m_Fuser = signInTask.Result;

            m_UserName.text = m_Fuser.DisplayName;
            m_UserId.text = m_Fuser.UserId;
            m_UserPP.text = m_Fuser.PhotoUrl.OriginalString;
        }));
    }

    public void SignOut()
    {
        if (m_Fuser != null)
        {
            var auth = FirebaseAuth.DefaultInstance;
            auth.SignOut();
            m_Fuser = null;
        }
    }
}
