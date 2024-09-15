using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DemoBlast.UI;

using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ProfileViewController : MonoBehaviour
{
    [SerializeField] private cView m_View;
    [SerializeField] private Button m_Button;
    [SerializeField] private RawImage m_RawImage;

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
                
                byte[] bytes = ImageConversion.EncodeToJPG(DuplicateTexture(texture));

                if (!Directory.Exists(Application.dataPath + "/SavedProfileImages"))
                {
                    Directory.CreateDirectory(Application.dataPath + "/SavedProfileImages");
                }

                // Write the returned byte array to a file in the project folder
                File.WriteAllBytes(Application.dataPath + "/SavedProfileImages/SavedScreen.jpg", bytes);

#if UNITY_EDITOR
                EditorUtility.RevealInFinder(Application.dataPath);
#endif
            }
        } );

        Debug.Log( "Permission result: " + permission );
    }
    
    Texture2D DuplicateTexture(Texture2D source)
    {
        var width = 128;
        var height = 128;
        
        RenderTexture renderTex = RenderTexture.GetTemporary(
            width,
            height,
            0,
            RenderTextureFormat.Default,
            RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(width, height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }
}
