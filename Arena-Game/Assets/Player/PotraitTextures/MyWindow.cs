using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

public class MyWindow : EditorWindow
{
    int objectNumber = 0;
    
    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/CombineTextures")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        MyWindow window = (MyWindow)EditorWindow.GetWindow(typeof(MyWindow));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Combiner Metallic and Roughness Textures", EditorStyles.boldLabel);

        if (GUILayout.Button("Create Metallic+Smoothness Map"))
        {
            string path = EditorUtility.OpenFolderPanel("Load png Textures", "", "");
            path = path.Substring(path.IndexOf("Assets"));
            objectNumber = 0;
            DirectoryInfo dir = new DirectoryInfo(path);
            ConvertFbxToPBR(dir);
        }
    }

    private void ConvertFbxToPBR(DirectoryInfo directory)
    {
        foreach (var VARIABLE in directory.GetDirectories())
        {
            ConvertFbxToPBR(VARIABLE);
        }
        
        if(directory.GetFiles().Length > 2) return;
        
        FileInfo[] info = directory.GetFiles("*.fbx");
        foreach (var f in info) 
        {
            var currentFilePath = f.FullName.Substring(f.FullName.IndexOf("Assets"));
            var currentDirectoryPath = f.DirectoryName.Substring(f.DirectoryName.IndexOf("Assets"));
            
            ExtractTextures(currentFilePath, currentDirectoryPath);

            AssetDatabase.Refresh();

            CustomModelImporter.ExtractMaterials(currentFilePath, currentDirectoryPath);
            
            var mats = directory.GetFiles("*.mat");
            
            foreach (var VARIABLE in mats)
            {
                var folder = Directory.CreateDirectory(VARIABLE.DirectoryName + "/" + Path.GetFileNameWithoutExtension(VARIABLE.Name)); //
                
                AssetDatabase.Refresh();
                
                File.Move(VARIABLE.FullName, folder.FullName + "/" + VARIABLE.Name);
                File.Move(VARIABLE.FullName + ".meta", folder.FullName + "/" + VARIABLE.Name + ".meta");

                foreach (var v in directory.GetFiles("*.png"))
                {
                    var n = Path.GetFileNameWithoutExtension(v.Name);
                    var matFileName = Path.GetFileNameWithoutExtension(VARIABLE.Name);
                    
                    if(n.Length < matFileName.Length) continue;
                    
                    if (n.Substring(0, n.IndexOf('_')) == matFileName)
                    {
                        File.Move(v.FullName, folder.FullName + "/" + v.Name);
                        File.Move(v.FullName + ".meta", folder.FullName + "/" + v.Name + ".meta");
                    }
                }

                AssetDatabase.Refresh();
                
                ConvertToNormalMap(folder);
                
                AssetDatabase.Refresh();
                
                CreateMetallicSmoothnessMap(folder, folder.FullName.Substring(folder.FullName.IndexOf("Assets")));
                
                AssetDatabase.Refresh();
                
                SetTextureMaps(folder);
                
                AssetDatabase.Refresh();
            }

            InstantiateAtScene(currentFilePath);
        }
    }

    private static void ExtractTextures(string currentFilePath, string currentDirectoryPath)
    {
        var modelImporter = AssetImporter.GetAtPath(currentFilePath) as ModelImporter;
        modelImporter.ExtractTextures(currentDirectoryPath);
    }

    private void InstantiateAtScene(string currentFilePath)
    {
        Object prefab = AssetDatabase.LoadAssetAtPath<Object>(currentFilePath);
        Instantiate(prefab, new Vector3(objectNumber + 20, 0, 6.5f), Quaternion.identity);
        objectNumber += 2;
    }

    private static void CreateMetallicSmoothnessMap(DirectoryInfo directory, string currentDirectoryPath)
    {
        var allPngs = directory.GetFiles("*.png");

        var metallicTextureFile = allPngs.Where((fileInfo => fileInfo.Name.Contains("metallic"))).FirstOrDefault();
        var roughnessTextureFile = allPngs.Where((fileInfo => fileInfo.Name.Contains("roughness"))).FirstOrDefault();
        var aoTextureFile = allPngs.Where((fileInfo => fileInfo.Name.Contains("ao"))).FirstOrDefault();
        
        if(metallicTextureFile == null && roughnessTextureFile == null && aoTextureFile == null) return;

        string name ="";
        
        
        Texture2D aoTexture =Texture2D.whiteTexture;
        if (aoTextureFile != null)
        {
            aoTexture = CustomModelImporter.LoadPNG(aoTextureFile.FullName);
            name = aoTexture.name;
            name = name.Replace("ao", "");
        }
         
        
        Texture2D metallicTexture = Texture2D.blackTexture;
        if (metallicTextureFile != null)
        {
            metallicTexture = CustomModelImporter.LoadPNG(metallicTextureFile.FullName);
            name = metallicTexture.name;
            name = name.Replace("metallic", "");
        }
        
        Texture2D roughnessTexture =Texture2D.whiteTexture;
        if (roughnessTextureFile != null)
        {
            roughnessTexture = CustomModelImporter.LoadPNG(roughnessTextureFile.FullName);
            name = roughnessTexture.name;
            name = name.Replace("roughness", "");
        }
            

        Texture2D combinedTexture = new Texture2D(metallicTexture.width, metallicTexture.height);

        for (int i = 0; i < combinedTexture.width; i++)
        {
            for (int j = 0; j < combinedTexture.height; j++)
            {
                var metallic = metallicTexture.GetPixel(i % metallicTexture.width, j % metallicTexture.height).r;
                var smoothness = (1 - roughnessTexture.GetPixel(i % roughnessTexture.width, j % roughnessTexture.width).r);
                var ao = aoTexture.GetPixel(i % aoTexture.width, j % aoTexture.height).r;
                combinedTexture.SetPixel(i, j, new Color(metallic, ao, 0, smoothness));
            }
        }

        combinedTexture.Apply();

        byte[] bytes = ((Texture2D)combinedTexture).EncodeToPNG();
        var dirPath = currentDirectoryPath;
        
        
        File.WriteAllBytes(dirPath + "/" + name + "CombinedMetallicSmoothness" + ".png", bytes);
    }

    private static void ConvertToNormalMap(DirectoryInfo dir)
    {
        var normalMapTexture = dir.GetFiles("*.png").Where((fileInfo => fileInfo.Name.Contains("normal"))).FirstOrDefault();
        
        if(normalMapTexture == null) return;
        
        var normalMapPath = normalMapTexture.FullName.Substring(normalMapTexture.FullName.IndexOf("Assets"));
        
        TextureImporter importSettings = (TextureImporter)AssetImporter.GetAtPath(normalMapPath);
        importSettings.textureType = TextureImporterType.NormalMap;
        AssetDatabase.ImportAsset(normalMapPath, ImportAssetOptions.ForceUpdate);
    }

    private void SetTextureMaps(DirectoryInfo VARIABLE)
    {
        SetTexture(VARIABLE, "color", "_BaseMap");
        SetTexture(VARIABLE, "CombinedMetallicSmoothness", "_MetallicGlossMap");
        SetTexture(VARIABLE, "normal", "_BumpMap");
        SetTexture(VARIABLE, "ao", "_OcclusionMap");
    }

    private void SetTexture(DirectoryInfo VARIABLE, string fileName, string textureName)
    {
        var assetPath = VARIABLE.GetFiles("*.mat").FirstOrDefault().FullName;
        Material material = AssetDatabase.LoadAssetAtPath<Material>(assetPath.Substring(assetPath.IndexOf("Assets")));
        
        material.SetFloat("_Smoothness", .15f);

        var image = VARIABLE.GetFiles("*.png")
            .Where((fileInfo => fileInfo.Name.Contains(fileName))).FirstOrDefault();

        if (image == null)
        {
            material.SetFloat(textureName, 0);
            return;
        }

        var fullName = image.FullName;
        Texture2D mainTex = AssetDatabase.LoadAssetAtPath<Texture2D>(fullName.Substring(fullName.IndexOf("Assets")));

        
        material.SetTexture(textureName, mainTex);
    }
}


public class CustomModelImporter
{
    public static void ExtractMaterials(string assetPath, string destinationPath)
    {
        HashSet<string> hashSet = new HashSet<string>();
        IEnumerable<Object> enumerable = from x in AssetDatabase.LoadAllAssetsAtPath(assetPath)
            where x.GetType() == typeof(Material)
            select x;
        foreach (Object item in enumerable)
        {
            string path = System.IO.Path.Combine(destinationPath, item.name) + ".mat";
            path = AssetDatabase.GenerateUniqueAssetPath(path);
            string value = AssetDatabase.ExtractAsset(item, path);
            if (string.IsNullOrEmpty(value))
            {
                hashSet.Add(assetPath);
            }
        }
 
        foreach (string item2 in hashSet)
        {
            AssetDatabase.WriteImportSettingsIfDirty(item2);
            AssetDatabase.ImportAsset(item2, ImportAssetOptions.ForceUpdate);
        }
    }

    public static Texture2D LoadPNG(string filePath) {
 
        Texture2D tex = null;
        byte[] fileData;
 
        Debug.Log(filePath+File.Exists(filePath));
        if (File.Exists(filePath))     {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }
}
#endif

