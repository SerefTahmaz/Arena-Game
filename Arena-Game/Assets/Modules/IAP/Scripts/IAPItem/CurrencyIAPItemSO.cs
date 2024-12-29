using ArenaGame.Currency;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "CurrencyIAPItemSO", menuName = "Game/IAP/CurrencyIAPItemSO", order = 0)]
    public class CurrencyIAPItemSO : BaseIAPItemSO
    {
        [SerializeField] private int m_Amount;

        public override void GiveReward()
        {
            base.GiveReward();
            CurrencyManager.GainCurrency(m_Amount);
        }
    }
}

