using System;
using Authentication;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ArenaGame.Managers.SaveManager
{
    [Serializable]
    public class UtilitySaveData
    {
        public int m_CurrentTierIndex;
        public int m_TierWins;
        public int m_TierLoses;
        public int m_FreeroamPlayCount;
        public float m_MasterVolume=1;
        public float m_SfxVolume=1;
        public float m_MusicVolume=1;
    }
    
    public static class UtilitySaveHandler
    {
        private static UtilitySaveData m_SaveData = new UtilitySaveData();
        public static UtilitySaveData SaveData
        {
            get => m_SaveData;
            set => m_SaveData = value;
        }

        public static bool m_Loaded = false;

        public static Action OnChanged = delegate { };

        public static async UniTask Load(){
            if(m_Loaded) return;
 
            if (AuthManager.Instance.IsAuthenticated)
            { 
                var utilitySaveData = await UtilitySaveService.FetchUtility(AuthManager.Instance.Uid);
                if (utilitySaveData == null)
                {
                    utilitySaveData = new UtilitySaveData();
                }
                SaveData = utilitySaveData;
                m_Loaded = true;

                OnChanged?.Invoke();
            }
        }
        
        public static async UniTask Save()
        {
            if (!m_Loaded)
            {
                Load();
            }

            if (AuthManager.Instance.IsAuthenticated)
            {
                UtilitySaveService.UpdateUtility(AuthManager.Instance.Uid,SaveData);

                OnChanged.Invoke();
            }
        }
    }
}