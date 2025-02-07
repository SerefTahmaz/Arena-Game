using Item.Scripts;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SerializableScriptableObject : ScriptableObject
{
    [SerializeField] Guid _guid;
    public Guid Guid => _guid;

#if UNITY_EDITOR
    void OnValidate()
    {
        var path = AssetDatabase.GetAssetPath(this);
        _guid = new Guid(AssetDatabase.AssetPathToGUID(path));
        OnValidateHelper();
    }
#endif

    protected virtual void OnValidateHelper()
    {
        
    }

    public void RegenerateGuid()
    {
        _guid = new Guid(System.Guid.NewGuid().ToString("N"));
    }

    public void SetGuid(string guid)
    {
        _guid = new Guid(guid);
    }
}