namespace ArenaGame.Managers.SaveManager
{
    public class SaveData
    {
        public string m_PlayerName = "NewPlayer";
        public bool m_IsPlayerSetName;
        public int m_Currency;
        public int m_ExperiencePoint;
        public int m_WinsCount;
        public bool m_AudioState = true;
        public bool m_MusicState = true;
        public bool m_HapticState = true;
        public int m_CurrentLevel;
    }

    public static class SaveDataUtility
    {
        public static SaveData CreateSaveData()
        {
            return new SaveData();
        }
    }
}