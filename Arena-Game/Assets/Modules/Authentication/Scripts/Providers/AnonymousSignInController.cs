using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using Authentication;
using Cysharp.Threading.Tasks;
using Firebase.Extensions;
using Firebase.Auth;
public class AnonymousSignInController : BaseAuthProvider
{
    [SerializeField] private cButton m_Button;
    
    public override async UniTask Init(IAuthService authService)
    {
        await base.Init(authService);
        m_Button.OnClickEvent.AddListener(HandleButtonClicked);
    }

    public void HandleButtonClicked() 
    {
        AnonymousLogin();
    }
    
    private async UniTask AnonymousLogin()
    {
        m_Button.DeActivate();

        var insInfoPopUp = GlobalFactory.InfoPopUpFactory.Create();
        await insInfoPopUp.Init(
            "Guest accounts are deleted after 30 days. If you want to save your progression, please user other sign in methods");

        MiniLoadingScreen.Instance.ShowPage(this);
        
        await LoginProcess();
        
        MiniLoadingScreen.Instance.HidePage(this);
        m_Button.Activate();
        
        AuthManager.Instance.AuthenticateUserAndConfigureUI();
    }

    private async UniTask LoginProcess()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        var task = auth.SignInAnonymouslyAsync();
        await task;
        
        if (task.IsCanceled)
        {
            Debug.LogError("SignInAnonymouslyAsync was canceled.");
            return;
        }
        if (task.IsFaulted)
        {
            Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
            return;
        }

        Debug.Log("Login Success");

        AuthResult result = task.Result;
        Debug.Log("Guest name: " + result.User.DisplayName);
        Debug.Log("Guest Id: " + result.User.UserId);
    }
  
}