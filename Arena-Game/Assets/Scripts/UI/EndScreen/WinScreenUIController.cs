using System.Collections.Generic;
using ArenaGame.Experience;
using ArenaGame.Managers.SaveManager;
using ArenaGame.UI;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;

namespace UI.EndScreen
{
    public class WinScreenUIController : MonoBehaviour
    {
        [SerializeField] private cButton m_ContinueButton;
        [SerializeField] private cView m_View;
        [SerializeField] private Transform m_Layout;
        [SerializeField] private List<BaseWinRewardSO> m_WinRewardSos;

        private List<IWinReward> m_SpawnedRewards = new List<IWinReward>();

        private void Awake()
        {
            m_ContinueButton.OnClickEvent.AddListener(HandleContinueClicked);
            m_View.OnActivateEvent.AddListener(OnViewActivated);
        }

        private void OnViewActivated()
        {
            OnViewActivatedAsync();
        }

        private async UniTask OnViewActivatedAsync()
        {
            m_ContinueButton.DOComplete();
            m_ContinueButton.DeActivate();
            m_ContinueButton.transform.localScale = Vector3.zero;
        
            foreach (var VARIABLE in m_SpawnedRewards)
            {
                Destroy(VARIABLE.transform.gameObject);
            }
        
            SaveGameHandler.Load();
            SaveGameHandler.SaveData.m_WinsCount += 1;
            SaveGameHandler.Save();
            ExperienceManager.GainExperience(30);
            await UniTask.WaitForSeconds(0.5f);

            foreach (var VARIABLE in m_WinRewardSos)
            {
                var insReward = VARIABLE.CreateRewardIns();
                insReward.transform.SetParent(m_Layout);
                m_SpawnedRewards.Add(insReward);
                await insReward.GiveReward();
            }

            m_ContinueButton.Activate();
            m_ContinueButton.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        }

        private void HandleContinueClicked()
        {
            cGameManager.Instance.HandleWinContinueButton();
        }
    }
}
