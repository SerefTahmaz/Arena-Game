using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ArenaGame;
using ArenaGame.Managers.SaveManager;
using ArenaGame.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuProfileController : MonoBehaviour
{
    [SerializeField] private cView m_View;
    [SerializeField] private Button m_Button; 
    [SerializeField] private RawImage m_RawImage;
    [SerializeField] private LogOutButtonController m_LogOutButtonController;
 
    private void Awake()
    {
        m_Button.onClick.AddListener(HandleImageClick);
        m_LogOutButtonController.OnLogOut += HandleLogOutButtonClicked;
        LoadProfile();
    }

    private void HandleLogOutButtonClicked()
    {
        m_View.Deactivate();
    }

    private void LoadProfile()
    {
        var profile = ProfileGenerator.GetPlayerProfile();
        
        var PPTex = profile.ProfilePicture;
        if (PPTex != null)
        {
            m_RawImage.texture = PPTex;
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