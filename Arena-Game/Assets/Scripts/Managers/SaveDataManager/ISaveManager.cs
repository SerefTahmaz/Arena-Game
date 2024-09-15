namespace ArenaGame.Managers.SaveManager
{
    public interface ISaveManager
    {
        public SaveData SaveData { get; set; }
        public void Save();
        public void Load();
    }
}