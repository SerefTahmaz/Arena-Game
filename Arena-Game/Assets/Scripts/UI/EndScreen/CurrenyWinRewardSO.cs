using UI.EndScreen;
using UnityEngine;
 
namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "MoneyWinReward", menuName = "Game/Rewards/CurrencyWinReward", order = 0)]
    public class CurrenyWinRewardSO : BaseWinRewardSO
    {
        [SerializeField] private int m_Amount;

        public override IWinReward CreateRewardIns()
        {
            base.CreateRewardIns();
            var ins = Instantiate(PrefabList.Get().CurrencyRewardPrefab);
            ins.Init(m_Amount);
            return ins;
        }
    }
}