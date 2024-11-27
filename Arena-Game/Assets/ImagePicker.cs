using System.Collections;
using System.Collections.Generic;
using ArenaGame;
using ArenaGame.UI;
using ArenaGame.Utils;
using UnityEngine;
using UnityEngine.UI;

public class ImagePicker : MonoBehaviour
{
    [SerializeField] private Button m_Button;
    [SerializeField] private RawImage m_RawImage;
    
    public Texture2D Image { get; set; }

    private void Awake()
    { 
        m_Button.onClick.AddListener(HandleImageClick);
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
                Image = texture.DuplicateTexture();
            }
        } );

        Debug.Log( "Permission result: " + permission );
    }
}
