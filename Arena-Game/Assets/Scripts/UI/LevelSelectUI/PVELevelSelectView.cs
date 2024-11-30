using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArenaGame.Managers.SaveManager;
using ArenaGame.UI;
using ArenaGame.Utils;
using UnityEngine;

public class PVELevelSelectView : cSingleton<PVELevelSelectView>
{
    [SerializeField] private PVELevelListSO m_LevelListSo;
    [SerializeField] private cLevelSelectUnit m_LevelSelectUnitPrefab;
    [SerializeField] private Transform m_Layout;
    [SerializeField] private cView m_View;

    private int m_CurrentIndex;
    public List<cLevelSelectUnit> m_LevelSelectUnits = new List<cLevelSelectUnit>();
    public cLevelSelectUnit m_SelectedLevelUnit;
    
    public cLevelSelectUnit SelectedLevelUnit => m_SelectedLevelUnit;

    public void Init()
    {
        foreach (var VARIABLE in m_LevelSelectUnits)
        {
            Destroy(VARIABLE.gameObject);
        }
        m_LevelSelectUnits.Clear();
        
        int currentLevel = SaveGameHandler.SaveData.m_CurrentPVELevel;
        m_CurrentIndex = currentLevel;
        for (var index = 0; index < m_LevelListSo.LevelList.Count; index++)
        {
            var levelSO = m_LevelListSo.LevelList[index];
            var ins = Instantiate(m_LevelSelectUnitPrefab, m_Layout);
            ins.Init(levelSO, index+1);
            ins.m_OnClick += OnSelect;
            m_LevelSelectUnits.Add(ins);
            if (m_CurrentIndex < index)
            {
                ins.SetLock(true);
            }
        }

        m_SelectedLevelUnit = m_LevelSelectUnits[m_CurrentIndex % m_LevelSelectUnits.Count];
        m_SelectedLevelUnit.SetSelected(true);
    }

    public void OnSelect(cLevelSelectUnit selectedUnit)
    {
        m_LevelSelectUnits.Except(new []{selectedUnit}).ForEach((unit => unit.SetSelected(false)));
        selectedUnit.SetSelected(true);
        m_SelectedLevelUnit = selectedUnit;
        
        m_CurrentIndex=m_LevelSelectUnits.IndexOf(m_SelectedLevelUnit);
    }

    public void OnStartSelected()
    {
        OnStartSelectedSinglePlayer();
    }

    private void OnStartSelectedSinglePlayer()
    {
        LoadingScreen.Instance.ShowPage(this);
        cLobbyManager.Instance.m_OnGameStarted += HandleOnGameStartedSingle;
        
        void Created()
        {
            cLobbyManager.Instance.UpdateIsPlayerReadyRateLimited(true);
        }
        cLobbyCreationManager.Instance.OnCreate(new cLobbyCreationManager.LobbyCreationSettingWrapper()
        {
            m_LobbyName = "SinglePlayerLobby", 
            m_PlayerCount = 1, 
            m_IsPrivate = true, 
            m_GameMode = eGameMode.PvE
        }, Created);
    }

    private void HandleOnGameStartedSingle()
    {
        cLobbyManager.Instance.m_OnGameStarted -= HandleOnGameStartedSingle;
        LoadingScreen.Instance.HidePage(this);
    }

    public void Activate()
    {
        m_View.Activate();
        Init();
    }

    public void SelectNext()
    {
        m_CurrentIndex++;
        if (m_CurrentIndex > SaveGameHandler.SaveData.m_CurrentPVELevel)
        {
            SaveGameHandler.SaveData.m_CurrentPVELevel = m_CurrentIndex;
            SaveGameHandler.Save();
        }
        
        OnSelect(m_LevelSelectUnits[m_CurrentIndex % m_LevelSelectUnits.Count]);
    }
}