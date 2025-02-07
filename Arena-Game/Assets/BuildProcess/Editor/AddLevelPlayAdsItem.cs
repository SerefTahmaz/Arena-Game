using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

public class AddLevelPlayAdsItem {
    
    [PostProcessBuildAttribute]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
        
        if (target == BuildTarget.iOS)
        {
            string plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
            
            PlistElementDict rootDict = plist.root;
            var arr = rootDict.CreateArray("SKAdNetworkItems");
            var dict = arr.AddDict();
            dict.SetString("SKAdNetworkIdentifier","su67r6k2v3.skadnetwork");

            var appSecurityDict = rootDict.CreateDict("NSAppTransportSecurity");
            appSecurityDict.SetBoolean("NSAllowsArbitraryLoads",true);
            
            File.WriteAllText(plistPath, plist.WriteToString());
            
            Debug.Log("Info.plist updated");
        }
        
    }
}