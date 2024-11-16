using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArenaGame.Managers.SaveManager;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Managers;
using RootMotion;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : Singleton<MapManager>
{
    [SerializeField] private string m_FreeroamLevel;
    [SerializeField] private PrewarmHelper m_PreWarmObject;
    
    private List<MapSO> Maps => MapListSO.Get().MapSOs;

    private int? m_CurrentLevel;
    private bool m_IsFreeroamLoaded;
    
    // Start is called before the first frame update
    void Start()
    {
        SaveGameHandler.Load();
        SetMap(SaveGameHandler.SaveData.m_CurrentMap);
    }

    public async UniTask SetMap(int levelIndex)
    {
        cUIManager.Instance.ShowPage(Page.Loading);
        await RemoveCurrentLevel();
        await UnloadFreeroam();
        
        m_CurrentLevel = levelIndex;
        await SceneManager.LoadSceneAsync(Maps[m_CurrentLevel.Value].SceneName, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(Maps[m_CurrentLevel.Value].SceneName));
        await PrewarmShaders(levelIndex);

        cUIManager.Instance.HidePage(Page.Loading);
    }

    private async Task PrewarmShaders(int index)
    {
        var insPrewarm = Instantiate(m_PreWarmObject);
        await insPrewarm.PrewarmShaders(index);
    }

    private async Task RemoveCurrentLevel()
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
        cUIManager.Instance.ShowPage(Page.Loading);
        
        await UnloadFreeroam();
        await RemoveCurrentLevel();
        
        m_IsFreeroamLoaded = true;
        await SceneManager.LoadSceneAsync(m_FreeroamLevel, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(m_FreeroamLevel));
        await PrewarmShaders(-1);
        cUIManager.Instance.HidePage(Page.Loading);
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
}
