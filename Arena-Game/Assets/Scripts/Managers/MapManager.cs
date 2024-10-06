using System.Collections;
using System.Collections.Generic;
using ArenaGame.Managers.SaveManager;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using RootMotion;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : Singleton<MapManager>
{
    [SerializeField] private string m_FreeroamLevel;
    
    private List<GameObject> m_InsMaps = new List<GameObject>();
    
    private List<MapSO> Maps => MapListSO.Get().MapSOs;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var VARIABLE in Maps)
        {
            var insMap = Instantiate(VARIABLE.MapPrefab);
            m_InsMaps.Add(insMap);
            insMap.SetActive(false);
            insMap.transform.SetParent(transform);
        }
        
        SaveGameHandler.Load();
        SetMap(SaveGameHandler.SaveData.m_CurrentMap);
    }

    public void SetMap(int levelIndex)
    {
        CloseLevels();
        m_InsMaps[levelIndex].SetActive(true);
    }

    public async UniTask LoadFreeroamLevel()
    {
        CloseLevels();
        cUIManager.Instance.ShowPage(Page.Loading);
        await SceneManager.LoadSceneAsync(m_FreeroamLevel, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(m_FreeroamLevel));
        cUIManager.Instance.HidePage(Page.Loading);
    }

    private void CloseLevels()
    {
        foreach (var ins in m_InsMaps)
        {
            ins.SetActive(false);
        }
    }
}
