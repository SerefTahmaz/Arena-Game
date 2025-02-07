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

public class MatchMakingController : MonoBehaviour
{
    [SerializeField] private GameObject m_EventSystem;
    [SerializeField] private EnemyCardController m_EnemyCardController;
    [SerializeField] private TMP_Text m_RewardCurrencyText;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static MatchMakingController CreateInstanceMatchMaking()
    {
        var ins = Instantiate(PrefabList.Get().MatchMakingController);
        return ins;
    }
    
    public async UniTask Init()
    {
        if (FindObjectsOfType<EventSystem>().Length > 1)
        {
            Destroy(m_EventSystem);
        }
        SetRewardText();

        await m_EnemyCardController.PlaySearchingAnim();
        await UniTask.WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private void SetRewardText()
    {
        m_RewardCurrencyText.text = MapManager.Instance.GetCurrentMap().RewardCurrency.ToString();
    }
}
