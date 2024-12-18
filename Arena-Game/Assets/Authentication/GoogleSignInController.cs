using System.Threading.Tasks;
using Authentication;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Google;
using Mono.CSharp;
using UnityEngine;

public class GoogleSignInController : BaseAuthProvider
{
    [SerializeField] private cButton m_Button;
    
    private string api = "1068966136010-54pe02ev4aeeolbmc0prms92b3bnu46b.apps.googleusercontent.com";
    private GoogleSignInConfiguration m_Configuration;

    public override async UniTask Init(IAuthService authService)
    {
        await base.Init(authService);
        m_Configuration = new GoogleSignInConfiguration
        {
            WebClientId = api,
            RequestIdToken = true
        };
        m_Button.OnClickEvent.AddListener(HandleButtonClicked);
    }

    public void HandleButtonClicked()
    {
        SignIn();
    }

    public async UniTask SignIn()
    {
        m_Button.DeActivate();
        MiniLoadingScreen.Instance.ShowPage(this);
        
        GoogleSignIn.Configuration = m_Configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;
        var googleSignTask = GoogleSignIn.DefaultInstance.SignIn();
        await googleSignTask;
        await FinishSignIn(googleSignTask);
        m_Button.Activate();
        MiniLoadingScreen.Instance.HidePage(this);
        AuthManager.Instance.AuthenticateUserAndConfigureUI();
    }
    
    private async UniTask FinishSignIn(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted || task.IsCanceled)
        {
            Debug.Log("Google registration error");
            return;
        }

        Credential credential = GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
        var firebaseUser = await m_AuthService.SignInWithCredentialAsync(credential);
        if (firebaseUser != null)
        {
            Debug.Log($"Google sign in successful! {firebaseUser.UserId}");
        }
    }

    public void SignOut()
    {
        // if (m_Fuser != null)
        // {
        //     var auth = FirebaseAuth.DefaultInstance;
        //     auth.SignOut();
        //     m_Fuser = null;
        // }
    }
}
