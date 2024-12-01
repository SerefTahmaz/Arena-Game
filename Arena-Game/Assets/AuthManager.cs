using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Managers.SaveManager;
using ArenaGame.UI;
using ArenaGame.Utils;
using Authentication;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using UnityEngine;

public class AuthManager : cSingleton<AuthManager>
{
    [SerializeField] private FirebaseAuthManager m_FirebaseAuthManager;
    [SerializeField] private LoginManager m_LoginManager;
    [SerializeField] private RegistrationManager m_RegistrationManager;
    [SerializeField] private cView m_View;
    
    public Action OnUserAuthenticated { get; set; }

    public IAuthService AuthService => m_FirebaseAuthManager;

    public string Uid => m_FirebaseAuthManager.User.UserId;
    public bool IsAuthenticated => m_FirebaseAuthManager.User != null;

    private void Awake()
    {
        Init();
    }

    private async UniTask Init()
    {
        LoadingScreen.Instance.ShowPage(this,true);
        await m_FirebaseAuthManager.Init();
        m_LoginManager.Init(m_FirebaseAuthManager);
        m_RegistrationManager.Init(m_FirebaseAuthManager);
        LoadingScreen.Instance.HidePage(this);
        // AuthenticateUserAndConfigureUI();
    }

    public void AuthenticateUserAndConfigureUI()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser == null)
        {
            m_View.Activate();
            m_LoginManager.ActivateUI();
        }
        else
        {
            OnUserAuthenticated?.Invoke();
            m_View.Deactivate();
            StartGame();
        }
    }

    private async UniTask StartGame()
    {
        SaveGameHandler.m_Loaded = false;
        await SaveGameHandler.Load();
        ItemSaveHandler.m_Loaded = false; 
        await ItemSaveHandler.Load();
        await CharacterSaveManager.Instance.Init();
        await cGameManager.Instance.StartMainScene();
    }

    public void SignOut()
    { 
        AuthService.SignOut();
        AuthenticateUserAndConfigureUI();
    }
}

