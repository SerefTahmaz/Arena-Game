using System.Collections.Generic;
using ArenaGame.Experience;
using ArenaGame.Managers.SaveManager;
using ArenaGame.UI;
using ArenaGame.Utils;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using DG.Tweening;
using Gameplay;
using TMPro;
using UnityEngine;

namespace UI.EndScreen
{
    public class WinScreenUIController : cSingleton<WinScreenUIController>
    {
        [SerializeField] private cButton m_ContinueButton;
        [SerializeField] private cView m_View;
        [SerializeField] private Transform m_Layout;
        [SerializeField] private CurrenyWinRewardSO m_CurrenyWinRewardSo;
        [SerializeField] private UpgradeWinRewardSO m_UpgradeWinRewardSo;
        [SerializeField] private CharacterSO m_PlayerChar;
        [SerializeField] private TMP_Text m_ExpRewardText;
        
        private List<IWinReward> m_SpawnedRewards = new List<IWinReward>();
        
        public int RewardExp { get; set; }

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
            m_SpawnedRewards.Clear(); 
        
            UserSaveHandler.Load();
            UserSaveHandler.SaveData.m_WinsCount += 1;
            UserSaveHandler.Save();

            var expRewawrd =RewardExp;
            ExperienceManager.GainExperience(expRewawrd);
            m_ExpRewardText.text = expRewawrd.ToString();
            
            await UniTask.WaitForSeconds(0.5f);

            m_SpawnedRewards = GenerateRewards();

            foreach (var winReward in m_SpawnedRewards)
            {
                winReward.transform.gameObject.SetActive(false);
            }
            
            foreach (var insReward in m_SpawnedRewards)
            {
                insReward.transform.gameObject.SetActive(true);
                await insReward.Spawn();
            }

            await UniTask.WaitForSeconds(0.25f);

            var rewardLocks = new List<UniTask>();
            foreach (var insReward in m_SpawnedRewards)
            {
                insReward.transform.gameObject.SetActive(true);
                rewardLocks.Add(insReward.GiveReward());
            }

            await UniTask.WhenAll(rewardLocks);

            m_ContinueButton.Activate();
            m_ContinueButton.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        }

        private List<IWinReward> GenerateRewards()
        {
            var generatedRewards = new List<IWinReward>();
            
            //Upgrade Reward
            var randomEquiptedItems = new List<ArmorItemSO>()
            {
                m_PlayerChar.GetCharacterSave().HelmArmor,
                m_PlayerChar.GetCharacterSave().ChestArmor,
                m_PlayerChar.GetCharacterSave().GauntletsArmor,
                m_PlayerChar.GetCharacterSave().LeggingArmor
            };
            randomEquiptedItems.RemoveAll((so => so == null));
            randomEquiptedItems.Shuffle();

            int maxAmount = Mathf.Min(2, randomEquiptedItems.Count);
            
            for (int i = 0; i < maxAmount; i++)
            {
                var insUpgradeReward = m_UpgradeWinRewardSo.CreateRewardIns(randomEquiptedItems[i]);
                insUpgradeReward.transform.SetParent(m_Layout);
                insUpgradeReward.transform.localScale = Vector3.one;
                generatedRewards.Add(insUpgradeReward);
            }
            
            //Currency Reward
            var insReward = m_CurrenyWinRewardSo.CreateRewardIns();
            insReward.transform.SetParent(m_Layout);
            insReward.transform.localScale = Vector3.one;
            generatedRewards.Add(insReward);
            
            return generatedRewards;
        }

        private void HandleContinueClicked()
        {
            cGameManager.Instance.HandleWinContinueButton();
        }
    }
}
