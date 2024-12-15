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
    [SerializeField] private AudioClip m_MultiplayerStartedClip;
    [SerializeField] private AudioClip m_CoinChangeSound;
    [SerializeField] private AudioClip m_ExpChangeSound;
    [SerializeField] private float m_Volume;

    private int LastCoinValue;
    private int LastExpValue;
       
    // Start is called before the first frame update
    void Start()
    {
        MultiplayerLocalHelper.Instance.OnMultiplayerGameStarted += HandleOnMultiplayerGameStarted;
        
        GameplayStatics.GetPlayerCharacterSO().GetCharacterSave().OnChanged += HandleCoinChange;
        LastCoinValue = GameplayStatics.GetPlayerCharacterSO().GetCharacterSave().Currency;

        SaveGameHandler.OnChanged += HandleExpChange;
        LastExpValue = ProfileGenerator.GetPlayerProfile().ExpPoint;
    }
    
    private void OnDestroy()
    {
        if (MultiplayerLocalHelper.Instance)
        {
            MultiplayerLocalHelper.Instance.OnMultiplayerGameStarted -= HandleOnMultiplayerGameStarted;
        }
        GameplayStatics.GetPlayerCharacterSO().GetCharacterSave().OnChanged -= HandleCoinChange;
        SaveGameHandler.OnChanged -= HandleExpChange;
    }

    private void HandleExpChange()
    {
        var expValue = ProfileGenerator.GetPlayerProfile().ExpPoint;
        if (LastExpValue != expValue)
        {
            LastExpValue = expValue;
            SoundManager.PlayOneShot2D(m_ExpChangeSound,m_Volume);
        }
    }

    private void HandleCoinChange()
    {
        var coinValue = GameplayStatics.GetPlayerCharacterSO().GetCharacterSave().Currency;
        if (LastCoinValue != coinValue)
        {
            LastCoinValue = coinValue;
            SoundManager.PlayOneShot2D(m_CoinChangeSound,m_Volume);
        }
    }

    private void HandleOnMultiplayerGameStarted()
    {
        SoundManager.PlayOneShot2D(m_MultiplayerStartedClip,m_Volume);
    }
}
