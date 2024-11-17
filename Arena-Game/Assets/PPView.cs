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
    }

    private void HandleOnChange()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        m_Image.texture = ProfileGenerator.GetPlayerProfile().ProfilePicture;
    }
}