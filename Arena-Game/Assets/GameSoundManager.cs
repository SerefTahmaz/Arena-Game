using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame;
using ArenaGame.Managers.SaveManager;
using AudioSystem;
using DefaultNamespace;
using UnityEngine;

public class GameSoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip m_CoinChangeSound;
    [SerializeField] private AudioClip m_ExpChangeSound;
    [SerializeField] private float m_Volume;

    private int m_LastCoinValue;
    private int m_LastExpValue;
       
    // Start is called before the first frame update
    void Start()
    {
        GameplayStatics.GetPlayerCharacterSO().GetCharacterSave().OnChanged += HandleCoinChange;
        m_LastCoinValue = GameplayStatics.GetPlayerCharacterSO().GetCharacterSave().Currency;

        SaveGameHandler.OnChanged += HandleExpChange;
        m_LastExpValue = ProfileGenerator.GetPlayerProfile().ExpPoint;
    }

    private void OnDestroy()
    {
        var characterSaveController = GameplayStatics.GetPlayerCharacterSO().GetCharacterSave();
        if (characterSaveController != null)
        {
            characterSaveController.OnChanged -= HandleCoinChange;
        }
            
        SaveGameHandler.OnChanged -= HandleExpChange;
    }

    private void HandleExpChange()
    {
        var expValue = ProfileGenerator.GetPlayerProfile().ExpPoint;
        if (m_LastExpValue != expValue)
        {
            m_LastExpValue = expValue;
            SoundManager.PlayOneShot2D(m_ExpChangeSound,m_Volume);
        }
    } 

    private void HandleCoinChange()
    {
        var coinValue = GameplayStatics.GetPlayerCharacterSO().GetCharacterSave().Currency;
        if (m_LastCoinValue != coinValue)
        {
            m_LastCoinValue = coinValue;
            SoundManager.PlayOneShot2D(m_CoinChangeSound,m_Volume);
        }
    }
}
 