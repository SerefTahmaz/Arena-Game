using ArenaGame;
using ArenaGame.Managers.SaveManager;
using UnityEngine;
using UnityEngine.UI;

public class EditProfileController : MonoBehaviour
{
    [SerializeField] protected cMenuNode m_MenuNode;
    [SerializeField] private Button m_Button; 
    [SerializeField] private RawImage m_RawImage;
    
    protected virtual void Awake()
    {
        m_Button.onClick.AddListener(HandleImageClick);
        m_MenuNode.OnActivateEvent.AddListener(LoadProfile);
        LoadProfile();
        SaveGameHandler.OnChanged += LoadProfile;
    }

    private void OnDestroy()
    {
        SaveGameHandler.OnChanged -= LoadProfile;
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
            m_RawImage.texture = PrefabList.Get().DefaultPPIcon.texture;
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