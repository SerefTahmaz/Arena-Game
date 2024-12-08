using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame;
using ArenaGame.Currency;
using ArenaGame.Managers.SaveManager;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PPView : MonoBehaviour
{
    [SerializeField] private RawImage m_Image;
    
    // Start is called before the first frame update
    void Start()
    {
        SaveGameHandler.OnChanged += HandleOnChange;
        UpdateUI();
    }

    private void OnDestroy()
    {
        SaveGameHandler.OnChanged -= HandleOnChange;
    }

    private void HandleOnChange()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        LoadProfile();
    }
    
    private void LoadProfile()
    {
        var profile = ProfileGenerator.GetPlayerProfile();
        
        var PPTex = profile.ProfilePicture;
        if (PPTex != null)
        {
            m_Image.texture = PPTex;
        }
        else
        {
            m_Image.texture = PrefabList.Get().DefaultPPIcon.texture;
        }
    }
}