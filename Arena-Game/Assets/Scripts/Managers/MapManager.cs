using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArenaGame.Managers.SaveManager;
using ArenaGame.Utils;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Managers;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapManager : cSingleton<MapManager>
{
    [SerializeField] private string m_FreeroamLevel;
    [SerializeField] private PrewarmHelper m_PreWarmObject;
     
    private List<MapSO> Maps => MapListSO.Get().MapSOs;

    private int? m_CurrentLevel;
    private bool m_IsFreeroamLoaded;
    private bool m_NetworkLoading;
    
    public Action<int> OnMapLoaded { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        SaveGameHandler.Load();
        SetMap(SaveGameHandler.SaveData.m_CurrentMap);
    }

    public async UniTask SetMap(int levelIndex)
    {
        LoadingScreen.Instance.ShowPage(this, true);
        await RemoveCurrentLevel();
        await UnloadFreeroam();
        
        m_CurrentLevel = levelIndex;
        await SceneManager.LoadSceneAsync(Maps[m_CurrentLevel.Value].SceneName, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(Maps[m_CurrentLevel.Value].SceneName));
        await PrewarmShaders(levelIndex);

        OnMapLoaded?.Invoke(levelIndex);
        LoadingScreen.Instance.HidePage(this);
    }
    
    public async UniTask SetMapNetwork(int levelIndex)
    {
        LoadingScreen.Instance.ShowPage(this,true);
        await RemoveCurrentLevel();
        await UnloadFreeroam();
        
        m_NetworkLoading = false;
        m_CurrentLevel = levelIndex;
        NetworkManager.Singleton.SceneManager.LoadScene(Maps[m_CurrentLevel.Value].SceneName, LoadSceneMode.Additive);
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadCompleted;
        await UniTask.WaitUntil((() => m_NetworkLoading));
        m_NetworkLoading = false;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(Maps[m_CurrentLevel.Value].SceneName));
        await PrewarmShaders(levelIndex);

        OnMapLoaded?.Invoke(levelIndex);
        LoadingScreen.Instance.HidePage(this);
    }

    private void OnLoadCompleted(string scenename, LoadSceneMode loadscenemode, List<ulong> clientscompleted, List<ulong> clientstimedout)
    {
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= OnLoadCompleted;
        m_NetworkLoading = true;
    }

    private async Task PrewarmShaders(int index)
    {
        var insPrewarm = Instantiate(m_PreWarmObject);
        await insPrewarm.PrewarmShaders(index);
    }

    public async Task RemoveCurrentLevel()
    {
        if (m_CurrentLevel != null)
        {
            await SceneManager.UnloadSceneAsync(Maps[m_CurrentLevel.Value].SceneName);
            m_CurrentLevel = null;
            await UniTask.WaitForSeconds(1);
            GC.Collect();
            Resources.UnloadUnusedAssets();
            await UniTask.WaitForSeconds(1);
        }
    }

    public async UniTask LoadFreeroamLevel()
    {
        LoadingScreen.Instance.ShowPage(this);
        
        await UnloadFreeroam();
        await RemoveCurrentLevel();
        
        m_IsFreeroamLoaded = true;
        await SceneManager.LoadSceneAsync(m_FreeroamLevel, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(m_FreeroamLevel));
        await PrewarmShaders(-1);
        
        OnMapLoaded?.Invoke(-1);
        LoadingScreen.Instance.HidePage(this);
    }

    public async UniTask UnloadFreeroam()
    {
        if (m_IsFreeroamLoaded)
        {
            await SceneManager.UnloadSceneAsync(m_FreeroamLevel);
            await UniTask.WaitForSeconds(1);
            GC.Collect();
            Resources.UnloadUnusedAssets();
            await UniTask.WaitForSeconds(1);
            m_IsFreeroamLoaded = false;
        }
    }

    public async UniTask SetMapIndex(int instanceLastMapIndex)
    {
        LoadingScreen.Instance.ShowPage(this,true);
        
        m_CurrentLevel = instanceLastMapIndex;
        await PrewarmShaders(instanceLastMapIndex);

        OnMapLoaded?.Invoke(instanceLastMapIndex);
        LoadingScreen.Instance.HidePage(this);
    }
}



#if UNITY_EDITOR
[CustomEditor(typeof(MapManager))]
public class MapManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Click"))
        {
            // (target as MapManager).MarkAsActive();
        }
    }
}
#endif