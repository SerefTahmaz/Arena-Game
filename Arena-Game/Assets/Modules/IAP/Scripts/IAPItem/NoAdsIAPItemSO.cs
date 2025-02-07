using ArenaGame.Currency;
using ArenaGame.Managers.SaveManager;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "NoAdsIAPItemSO", menuName = "Game/IAP/NoAdsIAPItemSO", order = 0)]
    public class NoAdsIAPItemSO : BaseIAPItemSO
    {
        public override void GiveReward()
        {
            base.GiveReward();
            UtilitySaveHandler.SaveData.m_NoAdsPurchased = true;
            UtilitySaveHandler.Save();
        }
    }
}