using ArenaGame.Currency;
using ArenaGame.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.EndScreen
{
    public class WinUpgradeRewardController : MonoBehaviour,IWinReward
    {
        [SerializeField] private Image m_Image;
        [SerializeField] private TMP_Text m_LevelText;
        [SerializeField] private TMP_Text m_LevelIncrementText;
        [SerializeField] private Image m_LevelIncrementFill;
        [SerializeField] private cView m_View;

        public ArmorItemSO itemTemplate;

        public void Init(ArmorItemSO itemTemplate)
        {
            this.itemTemplate = itemTemplate;
            m_Image.sprite = itemTemplate.ItemTemplate.ItemSprite;

            m_LevelText.text = $"LVL{this.itemTemplate.Level}";
            m_LevelIncrementText.text = $"{this.itemTemplate.NextLevelIncrement}/6";
            m_LevelIncrementFill.fillAmount = ((float)this.itemTemplate.NextLevelIncrement / 6);
        }

        public async UniTask Spawn()
        {
            await transform.DOScale(0.15f, 0.25f).SetLoops(2, LoopType.Yoyo).SetRelative(true);
        }
        
        public async UniTask GiveReward()
        {
            // CurrencyManager.GainCurrency(m_Amount);
            var startNextLevelIncrement = itemTemplate.NextLevelIncrement;
            var startLevel = itemTemplate.Level;

            var isIncrement5 = startNextLevelIncrement == 5;
            
            itemTemplate.IncreaseLevelIncrement();

            var animDuration = 1;
            
            var incrementLerpTarget = isIncrement5 ? 6 : itemTemplate.NextLevelIncrement;
            
            m_LevelIncrementFill.DOFillAmount(((float)incrementLerpTarget / 6), animDuration);
            await DOVirtual.Float(0, 1, animDuration, value =>
            {
                var lerpLevel = Mathf.CeilToInt(Mathf.Lerp(startLevel, itemTemplate.Level, value));
                var lerpNextLevelIncrement = Mathf.CeilToInt(Mathf.Lerp(startNextLevelIncrement, incrementLerpTarget, value));
                
                m_LevelText.text = $"LVL{lerpLevel}";
                m_LevelIncrementText.text = $"{lerpNextLevelIncrement}/6";
            }).OnComplete((() =>
            {
                m_LevelText.text = $"LVL{this.itemTemplate.Level}";
                m_LevelIncrementText.text = $"{this.itemTemplate.NextLevelIncrement}/6";
                m_LevelIncrementFill.fillAmount = ((float)this.itemTemplate.NextLevelIncrement / 6);
            }));
            
            await UniTask.WaitForSeconds(0.25f);
        }
    }
}