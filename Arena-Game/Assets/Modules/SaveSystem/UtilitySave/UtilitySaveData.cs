﻿using System;
using System.Collections.Generic;

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
        public float m_VoiceVolume=1;
        public int m_InterstitialShownCount;
        public bool m_NoAdsPurchased;
        public Dictionary<string, int> m_Purchases = new Dictionary<string, int>();
    }
}