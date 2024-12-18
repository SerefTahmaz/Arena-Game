using System;

namespace Authentication
{
    [Serializable]
    public class UserData
    {
        public string m_Username = "NewPlayer";
        public string m_Email;
        public string m_ProfileImageUrl;
        public int m_ExperiencePoint;
        public int m_WinsCount;
        public int m_CurrentPVELevel;
        public int m_CurrentMap;
        public bool m_IsPlayerDisqualified;
        public bool m_IsPlayerClosedAppInGameplay;
        public bool m_AudioState = true;
        public bool m_MusicState = true;
        public bool m_HapticState = true;
    }
}