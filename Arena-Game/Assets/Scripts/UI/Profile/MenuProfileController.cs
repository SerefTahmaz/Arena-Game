using System;
using ArenaGame;
using ArenaGame.Managers.SaveManager;
using ArenaGame.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MenuProfileController : MonoBehaviour
{
    [SerializeField] private cView m_View;
    [SerializeField] private Button m_Button; 
    [SerializeField] private RawImage m_RawImage;
    [SerializeField] private cButton m_DismissButton;
    
    private Texture m_DefaultSprite;
    private bool m_Dismissed;
 
    private void Awake()
    {
        m_DefaultSprite = m_RawImage.texture;
        
        m_Button.onClick.AddListener(HandleImageClick);
        m_View.OnActivateEvent.AddListener(LoadProfile);
        LoadProfile();
        UserSaveHandler.OnChanged += LoadProfile;
        
        m_DismissButton.OnClickEvent.AddListener(HandleOnDismissed);
    }

    public async UniTask WaitForDismiss()
    {
        m_Dismissed = false;
        UniTask.WaitUntil((() => m_Dismissed));
    }

    private void HandleOnDismissed()
    {
        m_Dismissed = true;
    }

    private void OnDestroy()
    {
        UserSaveHandler.OnChanged -= LoadProfile;
    }

    private void LoadProfile()
    {
        var profile = ProfileGenerator.GetPlayerProfile();
        
        var PPTex = profile.ProfilePicture;
        if (PPTex != null)
        {
            m_RawImage.texture = PPTex;
        }
        else
        {
            m_RawImage.texture = m_DefaultSprite;
        }
    }

    public void HandleImageClick()
    {
        PickImage(128);
    }
    
    private void PickImage( int maxSize )
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery( ( path ) =>
        {
            Debug.Log( "Image path: " + path );
            if( path != null )
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath( path, maxSize );
                if( texture == null )
                {
                    Debug.Log( "Couldn't load texture from " + path );
                    return;
                }

                m_RawImage.texture = texture;
                ProfileGenerator.SaveProfileImage(texture);
            }
        } );

        Debug.Log( "Permission result: " + permission );
    }
}