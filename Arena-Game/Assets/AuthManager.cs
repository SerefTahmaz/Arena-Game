using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    [SerializeField] private AuthProfileEditController m_AuthProfileEditController;
    
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
        LoadingScreen.Instance.HidePage(this);
        MiniLoadingScreen.Instance.ShowPage(this);
        List<UniTask> initTasks = new List<UniTask>();
        foreach (var authProvider in m_AuthProviders)
        {
            var task = authProvider.Init(m_FirebaseAuthManager);
            initTasks.Add(task);
        }

        await UniTask.WhenAll(initTasks);
        MiniLoadingScreen.Instance.HidePage(this);
        AuthenticateUserAndConfigureUI();
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
            StartGame();
        }
    } 

    private async UniTask StartGame()
    {
        MiniLoadingScreen.Instance.ShowPage(this);
        SaveGameHandler.m_Loaded = false;
        await SaveGameHandler.Load();
        ItemSaveHandler.m_Loaded = false; 
        await ItemSaveHandler.Load();
        await CharacterSaveManager.Instance.Init();
        
        var isFirstTime = await CheckFirstTimeUser();
        MiniLoadingScreen.Instance.HidePage(this);
        if (isFirstTime)
        {
          
            await m_AuthProfileEditController.Init();
        }
        foreach (var view in m_Views)
        {
            view.Deactivate();
        }
        await cGameManager.Instance.StartMainScene();
    }

    private async UniTask<bool> CheckFirstTimeUser()
    {
        var user = await UserService.FetchUser(FirebaseAuth.DefaultInstance.CurrentUser.UserId);
        if (user == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SignOut()
    { 
        AuthService.SignOut();
        AuthenticateUserAndConfigureUI();
    }
}