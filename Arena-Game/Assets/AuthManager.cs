using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using UnityEngine;
using Object = UnityEngine.Object;

public class AuthManager : cSingleton<AuthManager>
{
    [SerializeField] private FirebaseAuthManager m_FirebaseAuthManager;
    [SerializeField] private LoginManager m_LoginManager;
    [SerializeField] private RegistrationManager m_RegistrationManager;
    
    public Action OnUserAuthenticated { get; set; }

    public IAuthService AuthService => m_FirebaseAuthManager;

    private void Awake()
    {
        m_FirebaseAuthManager.Init();
        m_LoginManager.Init(m_FirebaseAuthManager);
        m_RegistrationManager.Init(m_FirebaseAuthManager);
    }

    public void AuthenticateUserAndConfigureUI()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser == null)
        {
            m_LoginManager.ActivateUI();
        }
        else
        {
            OnUserAuthenticated?.Invoke();
        }
    }

    public void SignOut()
    { 
        AuthService.SignOut();
        m_LoginManager.ActivateUI();
    }
}

