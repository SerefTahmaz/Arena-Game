using System.Collections;
using System.Collections.Generic;
using Authentication;
using Cysharp.Threading.Tasks;
using Extensions.Unity.ImageLoader;
using Facebook.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
 
public class FacebookSignInController : BaseAuthProvider
{
    [SerializeField] private cButton m_Button;
    
    private bool m_Initalized = false;
    
    public override async UniTask Init(IAuthService authService)
    {
        await base.Init(authService);
        m_Button.OnClickEvent.AddListener(HandleButtonClicked);
        LoadingScreen.Instance.ShowPage(this);

        m_Initalized = false;
        if (!FB.IsInitialized) {
            FB.Init(InitCallback, OnHideUnity);
        } else {
            // Already initialized
            FB.ActivateApp();
        }

        await UniTask.WaitUntil((() => m_Initalized));
        LoadingScreen.Instance.HidePage(this);
    }
    
    private void InitCallback ()
    {
        if (FB.IsInitialized) {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        } else {
            Debug.Log("Something went wrong to Initialize the Facebook SDK");
            gameObject.SetActive(false);
        }

        m_Initalized = true;
    }

    private void HandleButtonClicked()
    {
        LoginFB();
    }

    private void OnHideUnity (bool isGameScreenShown)
    {
        if (!isGameScreenShown) {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        } else {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }
    
    public async UniTask LoginFB (){
        MiniLoadingScreen.Instance.ShowPage(this);
        
        // Permission option list      https://developers.facebook.com/docs/facebook-login/permissions/
        var permissons = new List<string>()   {"email", "public_profile"};
        FB.LogInWithReadPermissions(permissons, AuthStatusCallback);
    }
    private void AuthStatusCallback (ILoginResult result) {
        MiniLoadingScreen.Instance.HidePage(this);
        if (FB.IsLoggedIn) {
            // AccessToken class will have session details
            var accessToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // current access token's User ID : aToken.UserId
            LoginViaFirebaseFacebook(accessToken.TokenString); 
        } else {
            Debug.Log("User cancelled login");
        }
    }

    private async UniTask LoginViaFirebaseFacebook (string accessToken){
        var loadingToken = new object();
        MiniLoadingScreen.Instance.ShowPage(loadingToken);
        
        Firebase.Auth.Credential credential = Firebase.Auth.FacebookAuthProvider.GetCredential(accessToken);
        var user = await m_AuthService.SignInWithCredentialAsync(credential);
        
        MiniLoadingScreen.Instance.HidePage(loadingToken);
        if(user == null) return;

        Debug.Log($"Facebook sign in successful! {user.UserId}");
        AuthManager.Instance.AuthenticateUserAndConfigureUI();
    }
    
    private async UniTask DisplayProfile()
    {
        await UniTask.WaitForFixedUpdate();
        var auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null) {
            string name = user.DisplayName;
            string email = user.Email;
            System.Uri photo_url = user.PhotoUrl;
            // The user's Id, unique to the Firebase project.
            string uid = user.UserId;

            // m_UserName.text = user.DisplayName;
            // m_profileImage.sprite = await ImageLoader.LoadSprite(user.PhotoUrl.OriginalString);
            
            FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);
        }

       
    }
    
    void DisplayProfilePic(IGraphResult result)
    {
        if (result.Texture != null)
        {
            Debug.Log("Profile Pic");
            // m_RawImage.texture = result.Texture;
            //if (FB_profilePic != null) FB_profilePic.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());
            /*JSONObject json = new JSONObject(result.RawResult);

            StartCoroutine(DownloadTexture(json["picture"]["data"]["url"].str, profile_texture));*/
        }
        else
        {
            Debug.Log(result.Error);
        }
    }
    
    void GetProfileData (){
        var auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null) {
            string name = user.DisplayName;
            string email = user.Email;
            System.Uri photo_url = user.PhotoUrl;
            // The user's Id, unique to the Firebase project.
            string uid = user.UserId;
        }
    }
    
    // void logout(){
    //     var auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    //     auth.SignOut();
    // }
}