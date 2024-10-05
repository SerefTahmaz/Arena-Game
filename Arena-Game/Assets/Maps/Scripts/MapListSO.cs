using System.Collections.Generic;
using _Main.Scripts;
using ArenaGame.Managers.SaveManager;
using ArenaGame.UI.PopUps.InfoPopUp;
using DefaultNamespace;
using UI.EndScreen;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Map List", menuName = "Game/Map/Map List")]
public class MapListSO : ScriptableObject
{
    [SerializeField] private List<MapSO> m_MapSOs;
    
    private static MapListSO m_Instance;

    public List<MapSO> MapSOs => m_MapSOs;

    public static MapListSO Get()
    {
        if (m_Instance == null)
        {
            m_Instance=Resources.Load<MapListSO>("Map List");
        }
            
        return m_Instance;
    }

    public static MapSO GetCurrentMap()
    {
        SaveGameHandler.Load();
        var currentLevel = Get().m_MapSOs[SaveGameHandler.SaveData.m_CurrentMap];
        return currentLevel;
    }
}