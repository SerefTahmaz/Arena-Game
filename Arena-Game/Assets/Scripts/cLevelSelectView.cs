using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DemoBlast.UI;
using DemoBlast.Utils;
using UnityEngine;

public class cLevelSelectView : cSingleton<cLevelSelectView>
{
    [SerializeField] private cLevelListSO m_LevelListSo;
    [SerializeField] private cLevelSelectUnit m_LevelSelectUnitPrefab;
    [SerializeField] private Transform m_Layout;
    [SerializeField] private cView m_View;
    [SerializeField] private cCreateLobbyUIController m_CreateLobbyUIController;

    private List<cLevelSelectUnit> m_LevelSelectUnits = new List<cLevelSelectUnit>();

    private cLevelSelectUnit m_SelectedLevelUnit;

    public cLevelSelectUnit SelectedLevelUnit => m_SelectedLevelUnit;

    private int m_CurrentIndex;

    // Start is called before the first frame update
    void Start()
    {
        int currentLevel = 1;
        m_CurrentIndex = currentLevel;
        for (var index = 0; index < m_LevelListSo.LevelList.Count; index++)
        {
            var levelSO = m_LevelListSo.LevelList[index];
            var ins = Instantiate(m_LevelSelectUnitPrefab, m_Layout);
            ins.Init(levelSO, index+1);
            ins.m_OnClick += OnSelect;
            m_LevelSelectUnits.Add(ins);
            if (currentLevel <= index)
            {
                ins.SetLock(true);
            }

            if (index + 1 == currentLevel)
            {
                OnSelect(ins);
            }
        }

        m_SelectedLevelUnit = m_LevelSelectUnits[0];
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
        m_View.Deactivate();
        Instantiate(m_CreateLobbyUIController, cUIManager.Instance.transform);
    }

    public void Activate()
    {
        m_View.Activate();
    }

    public void SelectNext()
    {
        m_CurrentIndex++;
        OnSelect(m_LevelSelectUnits[m_CurrentIndex % m_LevelSelectUnits.Count]);
    }
}