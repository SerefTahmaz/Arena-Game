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
        MiniLoadingScreen.Instance.ShowPage(this);
        m_Button.DeActivate();

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