using UI.EndScreen;
using UnityEngine;

namespace DefaultNamespace
{
    public class BaseWinRewardSO : ScriptableObject
    {
        public virtual IWinReward CreateRewardIns()
        {
            return null;
        }
    }
}