using System.IO;
using UnityEditor;
using UnityEngine;

namespace _Main.Scripts
{
    public class ScreenShot : MonoBehaviour
    {
        [ContextMenu("ss")]
        public void GenerateScreenShot()
        {
            RenderTexture renderTexture = new RenderTexture(256, 256, 24);
            Camera main = Camera.main;
            main.targetTexture = renderTexture;
            Texture2D ss = new Texture2D(256, 256, TextureFormat.RGBA32, false);
            main.Render();
            RenderTexture.active = renderTexture;
            ss.ReadPixels(new Rect(0,0,256,256),0,0);
            main.targetTexture = null;
            RenderTexture.active = null;

            if (Application.isEditor)
            {
                DestroyImmediate(renderTexture);
            }
            else
            {
                Destroy(renderTexture);
            }

            var bytes = ss.EncodeToPNG();
            var dirPath = Application.dataPath +"/Icons/";
            File.WriteAllBytes(dirPath +"image" + Random.Range(0,100000).ToString()+".png", bytes);

            #if UNITY_EDITOR
                        AssetDatabase.Refresh();
            #endif
        }
    }
}