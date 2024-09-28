using System.Collections;
using System.Collections.Generic;
using ArenaGame.Managers.SaveManager;
using DefaultNamespace;
using RootMotion;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
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
        foreach (var ins in m_InsMaps)
        {
            ins.SetActive(false);
        }
        
        m_InsMaps[levelIndex].SetActive(true);
    }
}
