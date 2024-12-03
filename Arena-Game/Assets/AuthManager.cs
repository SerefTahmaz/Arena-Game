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
using UnityEngine.Serialization;

public class AuthManager : cSingleton<AuthManager>
{
    [SerializeField] private FirebaseAuthManager m_FirebaseAuthManager;
    [SerializeField] private List<BaseAuthProvider> m_AuthProviders;
    [SerializeField] private List<cView> m_Views;
    
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
        foreach (var authProvider in m_AuthProviders)
        {
            authProvider.Init(m_FirebaseAuthManager);
        }
        LoadingScreen.Instance.HidePage(this);
        // AuthenticateUserAndConfigureUI();
    }

    public void AuthenticateUserAndConfigureUI()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser == null)
        {
            Debug.Log("No logged in user");
            foreach (var view in m_Views)
            {
                view.Activate();
            }
        }
        else
        {
            OnUserAuthenticated?.Invoke();
            foreach (var view in m_Views)
            {
                view.Deactivate();
            }
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

