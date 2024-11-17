using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Dialogue;
using UnityEngine;
using UnityEngine.EventSystems;

public class MatchMakingController : MonoBehaviour
{
    [SerializeField] private GameObject m_EventSystem;
    [SerializeField] private EnemyCardController m_EnemyCardController;

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

        await m_EnemyCardController.PlaySearchingAnim();
        await UniTask.WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
