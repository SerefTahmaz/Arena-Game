using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Extensions.Unity.ImageLoader;
using Facebook.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FacebookLogin : MonoBehaviour
{
    [SerializeField] private Image m_profileImage;
    [SerializeField] private RawImage m_RawImage;
    [SerializeField] private TMP_Text m_UserName;
    
    
    // Start function from Unity's MonoBehavior
    void Start ()
    {
        if (!FB.IsInitialized) {
            FB.Init(initCallback, onHideUnity);
        } else {
            // Already initialized
            FB.ActivateApp();
        }
    }

    private void initCallback ()
    {
        if (FB.IsInitialized) {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        } else {
            Debug.Log("Something went wrong to Initialize the Facebook SDK");
        }
    }

    private void onHideUnity (bool isGameScreenShown)
    {
        if (!isGameScreenShown) {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        } else {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }
    public void loginBtnForFB (){
// Permission option list      https://developers.facebook.com/docs/facebook-login/permissions/
        var permissons = new List<string>()   {"email", "public_profile"};
        FB.LogInWithReadPermissions(permissons, authStatusCallback);
    }
    private void authStatusCallback (ILoginResult result) {
        if (FB.IsLoggedIn) {
            // AccessToken class will have session details
            var accessToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // current access token's User ID : aToken.UserId
            loginviaFirebaseFacebook(accessToken.TokenString); 
            

        } else {
            Debug.Log("User cancelled login");
        }
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

            m_UserName.text = user.DisplayName;
            m_profileImage.sprite = await ImageLoader.LoadSprite(user.PhotoUrl.OriginalString);
            
            FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);
        }

       
    }
    
    void DisplayProfilePic(IGraphResult result)
    {
        if (result.Texture != null)
        {
            Debug.Log("Profile Pic");
            m_RawImage.texture = result.Texture;
            //if (FB_profilePic != null) FB_profilePic.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());
            /*JSONObject json = new JSONObject(result.RawResult);

            StartCoroutine(DownloadTexture(json["picture"]["data"]["url"].str, profile_texture));*/
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

    private void loginviaFirebaseFacebook (string accessToken){
        var auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Firebase.Auth.Credential credential =
            Firebase.Auth.FacebookAuthProvider.GetCredential(accessToken);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            DisplayProfile();
        });
    }
    
    void getProfileData (){
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
    
    void logout(){
        var auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.SignOut();
    }
}