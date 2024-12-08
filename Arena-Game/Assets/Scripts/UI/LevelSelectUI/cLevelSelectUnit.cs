using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class cLevelSelectUnit : MonoBehaviour
{
    [SerializeField] private GameObject m_SelectedUI;
    [SerializeField] private GameObject m_UnselectedUI;
    [SerializeField] private GameObject m_LockedUI;
    [SerializeField] private Image m_Icon;
    [SerializeField] private TMP_Text m_NameText;

    public PVELevelSO m_LevelSo;
    
    public Action<cLevelSelectUnit> m_OnClick = delegate(cLevelSelectUnit unit) {  };

    public bool m_Locked = false;

    public PVELevelSO LevelSo => m_LevelSo;

    public void Init(PVELevelSO levelSo, int order)
    {
        m_LevelSo = levelSo;
        SetSelected(false);

        m_Icon.sprite = levelSo.Icon;
        m_Icon.color = new Color(1, 1, 1, levelSo.Alpha);
        m_NameText.text = $"{levelSo.NameText} {order.ToString()}";
    }

    public void SetSelected(bool state)
    {
        m_UnselectedUI.SetActive(!state);
        m_SelectedUI.SetActive(state);
    }
    
    public void SetLock(bool state)
    {
        m_Locked = state;
        m_LockedUI.SetActive(state);
    }

    public void OnClick()
    {
        m_OnClick.Invoke(this);
    }
}
