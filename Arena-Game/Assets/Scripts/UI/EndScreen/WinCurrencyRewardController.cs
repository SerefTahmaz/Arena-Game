using ArenaGame.Currency;
using ArenaGame.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI.EndScreen
{
    public class WinCurrencyRewardController : MonoBehaviour,IWinReward
    {
        [SerializeField] private TMP_Text m_Text;
        [SerializeField] private cView m_View;

        private int m_Amount;
    
        public void Init(int amount)
        {
            m_Amount = amount;
            m_Text.text = m_Amount.ToString();
        }
        
        public async UniTask Spawn()
        {
            await transform.DOScale(0.15f, 0.25f).SetLoops(2, LoopType.Yoyo).SetRelative(true);
        }

        public async UniTask GiveReward()
        {
            CurrencyManager.GainCurrency(m_Amount);

            await UniTask.WaitForSeconds(0.25f);
        }
    }

    public interface IWinReward
    {
        public Transform transform { get; }
        public UniTask GiveReward();
        public UniTask Spawn();
    }
}