using DefaultNamespace;
using UnityEngine;

namespace UI.EndScreen
{
    [CreateAssetMenu(fileName = "UpgradeWinReward", menuName = "Game/Rewards/UpgradeWinReward", order = 0)]
    public class UpgradeWinRewardSO : BaseWinRewardSO
    {
        public IWinReward CreateRewardIns(ArmorItemSO armorItemSo)
        {
            base.CreateRewardIns();
            var ins = Instantiate(PrefabList.Get().WinUpgradeRewardPrefab);
            ins.Init(armorItemSo);
            return ins;
        }
    }
}