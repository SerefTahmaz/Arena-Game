using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Managers.SaveManager;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class FakeInterstitial : MonoBehaviour 
{
    [SerializeField] private GameObject m_EventSystem;
    [SerializeField] private cButton m_Close;

    private bool m_IsClosedClicked;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        m_Close.OnClickEvent.AddListener(HandCloseButtonClicked);
    }

    public static FakeInterstitial CreateInstance()
    {
        var ins = Instantiate(PrefabList.Get().FakeInterstitialPrefab);
        return ins;
    }
    
    public async UniTask Init()
    {
        if (FindObjectsOfType<EventSystem>().Length > 1)
        {
            Destroy(m_EventSystem);
        }

        await UniTask.WaitUntil((() => m_IsClosedClicked));
        Destroy(gameObject);
    }

    private void HandCloseButtonClicked()
    {
        m_IsClosedClicked = true;
    }
}