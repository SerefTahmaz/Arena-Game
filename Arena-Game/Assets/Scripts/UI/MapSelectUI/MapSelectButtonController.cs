using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Managers.SaveManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapSelectButtonController : MonoBehaviour
{
    [SerializeField] private Image m_PreviewSprite;
    [SerializeField] private TMP_Text m_MapNameText;
    
    // Start is called before the first frame update
    void Start()
    {
        RefreshUI();
        UserSaveHandler.OnChanged += RefreshUI;
    }

    private void OnDestroy()
    {
        UserSaveHandler.OnChanged -= RefreshUI;
    }

    private void RefreshUI()
    {
        var saveGame = UserSaveHandler.SaveData;

        var currentMapSo = MapListSO.Get().MapSOs[saveGame.m_CurrentMap];

        m_PreviewSprite.sprite = currentMapSo.PreviewIcon;
        m_MapNameText.text = currentMapSo.MapName;
    }
}
