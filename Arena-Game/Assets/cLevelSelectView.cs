using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DemoBlast.UI;
using DemoBlast.Utils;
using UnityEngine;

public class cLevelSelectView : MonoBehaviour
{
    [SerializeField] private cLevelListSO m_LevelListSo;
    [SerializeField] private cLevelSelectUnit m_LevelSelectUnitPrefab;
    [SerializeField] private Transform m_Layout;
    [SerializeField] private cView m_View;

    private List<cLevelSelectUnit> m_LevelSelectUnits = new List<cLevelSelectUnit>();

    private cLevelSelectUnit m_SelectedLevelUnit;

    // Start is called before the first frame update
    void Start()
    {
        int currentLevel = 5;
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
                ins.SetSelected(true);
            }
        }
    }

    public void OnSelect(cLevelSelectUnit selectedUnit)
    {
        m_LevelSelectUnits.Except(new []{selectedUnit}).ForEach((unit => unit.SetSelected(false)));
        selectedUnit.SetSelected(true);
        m_SelectedLevelUnit = selectedUnit;
    }

    public void OnStartSelected()
    {
        m_View.Deactivate();
        cGameManager.Instance.StartRound(m_SelectedLevelUnit.LevelSo);
    }
}